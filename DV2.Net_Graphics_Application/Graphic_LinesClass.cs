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

            if (commData[0].ToLower() == "line" && chkData[0] == "Line" && pointData.Count == 4)
            {
                DrawLine(ref pointData, picPen);
            }
            else if (commData[0].ToLower() == "dashline" && chkData[0] == "DashLine" && pointData.Count == 4)
            {
                DrawLine(ref pointData, picPen, true);
            }
            else
            {
                tobeRead.SpeakAsync("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjCommand.BinarySearch(backObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjCommand.BinarySearch(backObjComm)] + "対象定義識別失敗!");
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
            pointAx = Convert.ToSingle(pointData[0]) + offset;
            pointAy = Convert.ToSingle(pointData[1]) + offset;
            pointBx = Convert.ToSingle(pointData[2]) + offset;
            pointBy = Convert.ToSingle(pointData[3]) + offset;
            
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
                tobeRead.SpeakAsync("@Graphic_LinesClass Draw_ArrowMode関数" + ObjName[ObjCommand.BinarySearch(backObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_LinesClass Draw_ArrowMode関数" + ObjName[ObjCommand.BinarySearch(backObjComm)] + "対象定義識別失敗!");
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
            pointAx = Convert.ToSingle(pointData[0]) + offset;
            pointAy = Convert.ToSingle(pointData[1]) + offset;
            pointBx = Convert.ToSingle(pointData[2]) + offset;
            pointBy = Convert.ToSingle(pointData[3]) + offset;

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
