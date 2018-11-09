using System;

#region Personal Addition
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;
#endregion

namespace DV2.Net_Graphics_Application
{
    //class Graphic_LinesClass
    public partial class MainForm
    {
        public void Draw_LineMode(string ObjComm, string ObjAna)
        {
            //Debug
            //LogOutput("\r\n@Graphic_LinesClass  -> Draw_LineMode");
            //LogOutput("Parameter 1  -> " + ObjComm);
            //LogOutput("Parameter 2  -> " + ObjAna + "\r\n");
            //Parameter 1->obj1 |=| line | (| 0 |,| 5 |,| 200.0 |,| 405.0 |)
            //Parameter 2->Line | Lparen | IntNum | Comma | IntNum | Comma | DblNum | Comma | DblNum | Rparen
            //System.Windows.Forms.MessageBox.Show("Draw_LineMode");

            //Define
            double distance = 0.0;
            #region pointData.Count -> 5 用パラメーター
            double unKnownX = 0, unKnownY = 0;
            double slopeA = 0, InterceptB = 0, alpha = 0;
            #endregion
            string[] chkData, commData;
            string backObjComm = ObjComm;
            ArrayList pointData = new ArrayList();
            Pen picPen = new Pen(Color.Black, 0.1F);

            //Processing
            AssignRemover(ref ObjComm);
            commData = Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase);
            chkData = Regex.Split(ObjAna, @"\|", RegexOptions.IgnoreCase);
            
            for (int i = 0; i < chkData.Length; i++)
            {
                if (chkData[i] == "IntNum" || chkData[i] == "DblNum")
                {
                    pointData.Add(commData[i]);
                }
            }

            if (commData[0].ToLower() == "line" && chkData[0] == "Line" && pointData.Count == 4)
            {
                DrawLine(ref pointData, picPen);
            }
            else if (commData[0].ToLower() == "dashline" && chkData[0] == "DashLine" && pointData.Count == 4)
            {
                DrawLine(ref pointData, picPen, true);
            }
            else if (commData[0].ToLower() == "line" && chkData[0] == "Line" && pointData.Count == 5)
            {
                LogOutput("LIne new mode debugging");
                //distance = 0.0;
                distance = Math.Sqrt((Convert.ToDouble(pointData[0]) - Convert.ToDouble(pointData[3])) * (Convert.ToDouble(pointData[0]) - Convert.ToDouble(pointData[3])) + (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[4])) * (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[4])));
                //y = Ax + B
                slopeA = Math.Round((Convert.ToDouble(pointData[3]) - Convert.ToDouble(pointData[1])) / (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[0])), 5);
                //データの有効性判断
                if (IsInfinityOrNaN(ref slopeA))
                {
                    //垂直処理
                    //InterceptB = Convert.ToDouble(pointData[3]);
                    //alpha = Math.Atan(slopeA);

                    unKnownX = Convert.ToDouble(pointData[2]);
                    unKnownY = Convert.ToDouble(pointData[3]) + Convert.ToDouble(pointData[4]);
                }

                else if (!IsInfinityOrNaN(ref slopeA) && slopeA == 0)
                {
                    //平行処理
                    unKnownX = Convert.ToDouble(pointData[2]) + Convert.ToDouble(pointData[4]);
                    unKnownY = Convert.ToDouble(pointData[3]);
                }

                else
                {
                    //通常処理
                    InterceptB = Convert.ToDouble(pointData[3]) - slopeA * Convert.ToDouble(pointData[2]);
                    alpha = Math.Atan(slopeA);

                    unKnownX = Math.Round(Convert.ToDouble(pointData[2]) + Convert.ToDouble(pointData[4]) * Math.Cos(alpha), 2);
                    unKnownY = Math.Round(Convert.ToDouble(pointData[3]) + Convert.ToDouble(pointData[4]) * Math.Sin(alpha), 2);
                }

                ArrayList temp_pointData = new ArrayList();
                //Pen picPen = new Pen(Color.Black, 0.1F);
                temp_pointData.Add(pointData[0]); temp_pointData.Add(pointData[1]);
                temp_pointData.Add(unKnownX); temp_pointData.Add(unKnownY);

                DrawLine(ref temp_pointData, picPen);
            }
            else if (commData[0].ToLower() == "dashline" && chkData[0] == "DashLine" && pointData.Count == 5)
            {
                LogOutput("DashLIne new mode debugging");
                //distance = 0.0;
                distance = Math.Sqrt((Convert.ToInt32(pointData[0]) - Convert.ToInt32(pointData[3])) * (Convert.ToInt32(pointData[0]) - Convert.ToInt32(pointData[3])) + (Convert.ToInt32(pointData[2]) - Convert.ToInt32(pointData[4])) * (Convert.ToInt32(pointData[2]) - Convert.ToInt32(pointData[4])));
                //y = Ax + B
                slopeA = Math.Round((Convert.ToDouble(pointData[3]) - Convert.ToDouble(pointData[1])) / (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[0])), 5);
                //データの有効性判断
                if (IsInfinityOrNaN(ref slopeA))
                {
                    //垂直処理
                    //InterceptB = Convert.ToDouble(pointData[3]);
                    //alpha = Math.Atan(slopeA);

                    unKnownX = Convert.ToDouble(pointData[2]);
                    unKnownY = Convert.ToDouble(pointData[3]) + Convert.ToDouble(pointData[4]);
                }

                else if (!IsInfinityOrNaN(ref slopeA) && slopeA == 0)
                {
                    //平行処理
                    unKnownX = Convert.ToDouble(pointData[2]) + Convert.ToDouble(pointData[4]);
                    unKnownY = Convert.ToDouble(pointData[3]);
                }

                else
                {
                    //通常処理
                    InterceptB = Convert.ToDouble(pointData[3]) - slopeA * Convert.ToDouble(pointData[2]);
                    alpha = Math.Atan(slopeA);

                    unKnownX = Math.Round(Convert.ToDouble(pointData[2]) + Convert.ToDouble(pointData[4]) * Math.Cos(alpha), 2);
                    unKnownY = Math.Round(Convert.ToDouble(pointData[3]) + Convert.ToDouble(pointData[4]) * Math.Sin(alpha), 2);
                }

                ArrayList temp_pointData = new ArrayList();
                //Pen picPen = new Pen(Color.Black, 0.1F);
                temp_pointData.Add(pointData[0]); temp_pointData.Add(pointData[1]);
                temp_pointData.Add(unKnownX); temp_pointData.Add(unKnownY);
                
                DrawLine(ref temp_pointData, picPen, true);
            }
            else
            {
                tobeRead.SpeakAsync("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!"); 
                codeOutput("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
            }
        }

        private void DrawLine(ref ArrayList pointData, Pen picPen = null, bool dashFlag = false, float offset = pub_offSet)
        {
            LogOutput("DrawLine (" + pointData[0] + "," + pointData[1] + ") -> (" + pointData[2] + "," + pointData[3] + ")");
            //offset 変位量
            //pointData 座標点ArrayList,可変パラメータ
            picPen = picPen ?? pub_picPen;
            float pointAx = 0, pointAy = 0, pointBx = 0, pointBy = 0;

            //座標計算
            pointAx = Convert.ToSingle(pointData[0]) + offset + Point_Offset.X;
            pointAy = Convert.ToSingle(pointData[1]) + offset + Point_Offset.Y;
            pointBx = Convert.ToSingle(pointData[2]) + offset + Point_Offset.X;
            pointBy = Convert.ToSingle(pointData[3]) + offset + Point_Offset.Y;
            
            if(dashFlag)
            {
                picPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                float[] dashArray =
                {
                    5f,     //线长5个像素
                    3.5f,     //间断3.5个像素
                    15f,    //线长15个像素
                    3.5f      //间断3.5个像素
                };
                picPen.DashPattern = dashArray;
            }
            graphObj.DrawLine(picPen, pointAx, pointAy, pointBx, pointBy);
        }

        public void Draw_ArrowMode(string ObjComm, string ObjAna)
        {
            //Debug
            LogOutput("\r\n@Graphic_LinesClass  -> Draw_ArrowMode");
            LogOutput("Parameter 1  -> " + ObjComm);
            LogOutput("Parameter 2  -> " + ObjAna + "\r\n");
            //System.Windows.Forms.MessageBox.Show("Draw_LineMode");

            //Define
            string[] chkData, commData;
            string backObjComm = ObjComm;
            ArrayList pointData = new ArrayList();
            Pen picPen = new Pen(Color.Black, 1F);

            //Processing
            AssignRemover(ref ObjComm);
            commData = Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase);
            chkData = Regex.Split(ObjAna, @"\|", RegexOptions.IgnoreCase);
            
            for (int i = 0; i < chkData.Length; i++)
            {
                if (chkData[i] == "IntNum" || chkData[i] == "DblNum")
                {
                    pointData.Add(commData[i]);
                }
            }

            if (commData[0].ToLower() == "arrow" && chkData[0] == "Arrow" && pointData.Count == 4)
            {
                DrawArrow(ref pointData, picPen);
            }
            else if (commData[0].ToLower() == "dasharrow" && chkData[0] == "DashArrow" && pointData.Count == 4)
            {
                DrawArrow(ref pointData, picPen, true);
            }
            else
            {
                tobeRead.SpeakAsync("@Graphic_LinesClass Draw_ArrowMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_LinesClass Draw_ArrowMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
            }
        }

        private void DrawArrow(ref ArrayList pointData, Pen picPen = null, bool dashFlag = false, float offset = pub_offSet)
        {
            LogOutput("DrawArrow (" + pointData[0] + "," + pointData[1] + ") -> (" + pointData[2] + "," + pointData[3] + ")");
            //offset 変位量
            //pointData 座標点ArrayList,可変パラメータ
            picPen = picPen ?? pub_picPen;
            float pointAx = 0, pointAy = 0, pointBx = 0, pointBy = 0;

            //座標計算
            pointAx = Convert.ToSingle(pointData[0]) + offset + Point_Offset.X;
            pointAy = Convert.ToSingle(pointData[1]) + offset + Point_Offset.Y;
            pointBx = Convert.ToSingle(pointData[2]) + offset + Point_Offset.X;
            pointBy = Convert.ToSingle(pointData[3]) + offset + Point_Offset.Y;

            if (dashFlag)
            {
                picPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                float[] dashArray =
                {
                    5f,     //线长5个像素
                    3.5f,     //间断3.5个像素
                    15f,    //线长15个像素
                    3.5f      //间断3.5个像素
                };
                picPen.DashPattern = dashArray;
            }
            picPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            graphObj.DrawLine(picPen, pointAx, pointAy, pointBx, pointBy);
        }
    }
}
