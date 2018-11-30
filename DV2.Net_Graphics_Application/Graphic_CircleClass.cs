using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;
#endregion

namespace DV2.Net_Graphics_Application
{
    //class Graphic_CircleClass
    public partial class MainForm
    {
        public void Draw_CircleMode(string ObjComm, string ObjAna)
        {
            //ObjComm	"circle|(|c|,|37|)"
            //ObjAna	"Circle|Lparen|Ident|Comma|IntNum|Rparen"
            LogOutput("Draw_CircleMode");
            //Define
            string[] anaData, commData;
            string backObjComm = ObjComm;
            ArrayList pointData = new ArrayList();
            Pen picPen = new Pen(Color.Black, 0.1F);

            //Processing
            AssignRemover(ref ObjComm);
            commData = Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase);
            anaData = Regex.Split(ObjAna, @"\|", RegexOptions.IgnoreCase);

            for (int i = 0; i < anaData.Length; i++)
            {
                if (anaData[i] == "IntNum" || anaData[i] == "DblNum")
                {
                    pointData.Add(commData[i]);
                }
            }

            if(pointData.Count == 1)
            {
                //Define
                int index;
                string subIdent, subComm, subAna;
                string[] subCommData, subAnaData;
                ArrayList subPointData = new ArrayList();

                //Processing
                //anaData   "["circle", "(", "c", ",", "5", ")"]"
                index = IdentFinder(ref anaData);

                if (index == -1)
                {
                    tobeRead.SpeakAsync("円定義の中にミスが存在している、もう一度確認してください。");
                    return;
                }

                subIdent = commData[index];
                index = 0;

                foreach (string finder in ObjName)
                {
                    if (finder == subIdent)
                    {
                        break;
                    }
                    index += 1;
                }

                if (ObjName.Count == index)
                {
                    tobeRead.SpeakAsync("中心座標点の定義が存在しない、もう一度確認してください。");
                    LogOutput("中心座標点の定義が存在しない、もう一度確認してください。");
                    return;
                }

                if (ObjName[index].ToString() != subIdent)
                {
                    tobeRead.SpeakAsync("中心座標点の定義が存在しない、もう一度確認してください。");
                    LogOutput("中心座標点の定義が存在しない、もう一度確認してください。");
                    return;
                }

                subComm = ObjCommand[index].ToString();
                subAna = ObjAnalysis[index].ToString();
                AssignRemover(ref subComm);
                subCommData = Regex.Split(subComm, @"\|", RegexOptions.IgnoreCase);
                subAnaData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                for (int i = 0; i < subAnaData.Length; i++)
                {
                    if (subAnaData[i] == "IntNum" || subAnaData[i] == "DblNum")
                    {
                        subPointData.Add(subCommData[i]);
                    }
                }

                if(subCommData[0].ToLower() == "point" && subAnaData[0] == "Point" && subPointData.Count == 2)
                {
                    
                    if (commData[0].ToLower() == "circle" && anaData[0] == "Circle")
                    {
                        subPointData.Add(pointData[pointData.Count - 1]);
                        DrawCircleMode(ref subPointData, picPen);
                    }
                    else if (commData[0].ToLower() == "fillcircle" && anaData[0] == "FillCircle")
                    {
                        subPointData.Add(pointData[pointData.Count - 1]);
                        DrawCircleMode(ref subPointData, picPen, true);
                    }
                }
                else
                {
                    if (subPointData.Count != 2)
                    {
                        tobeRead.SpeakAsync("Error @Draw_CircleMode" + "座標点異常ある");
                    }
                    tobeRead.SpeakAsync("Error @Draw_CircleMode");
                }
            }

            else if (commData[0].ToLower() == "circle" && anaData[0] == "Circle" && pointData.Count == 3)
            {
                DrawCircleMode(ref pointData, picPen);
            }
            else if (commData[0].ToLower() == "fillcircle" && anaData[0] == "FillCircle" && pointData.Count == 3)
            {
                DrawCircleMode(ref pointData, picPen, true);
            }
            else
            {
                tobeRead.SpeakAsync("@Graphic_CircleClass Draw_CircleMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_CircleClass Draw_CircleMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
            }
        }
        
        private void DrawCircleMode(ref ArrayList pointData, bool fillFlag = false, float offset = pub_offSet)
        {
            LogOutput("DrawCircle (" + pointData[0] + "," + pointData[1] + ") -> (" + pointData[2] + ")");
            //offset 変位量
            //pointData 座標点ArrayList,可変パラメータ
            Pen picPen = pub_picPen;
            float pointCx = 0, pointCy = 0, width = 0, height = 0;

            //座標計算
            //pointData[0] 中心座標点 X
            //pointData[1] 中心座標点 Y
            //pointData[2] 半径
            //LogOutput(DateTime.Now.ToString("HH:mm") + " 画像円を描く");
            pointCx = Convert.ToInt32(pointData[0]) - Convert.ToInt32(pointData[2]) + offset + Point_Offset.X;
            pointCy = Convert.ToInt32(pointData[1]) - Convert.ToInt32(pointData[2]) + offset + Point_Offset.Y;
            width = Convert.ToInt32(pointData[2]) * 2;
            height = Convert.ToInt32(pointData[2]) * 2;

            if (fillFlag)
            {
                SolidBrush picBru = new SolidBrush(Color.Black);
                graphObj.FillEllipse(picBru, pointCx, pointCy, width, height);
            }
            else
            {
                //picPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                graphObj.DrawEllipse(picPen, pointCx, pointCy, width, height);
            }
        }

        private void DrawCircleMode(ref ArrayList pointData, Pen picPen = null, bool fillFlag = false, float offset = pub_offSet)
        {
            LogOutput("DrawCircle (" + pointData[0] + "," + pointData[1] + ") -> (" + pointData[2] + ")");
            //offset 変位量
            //pointData 座標点ArrayList,可変パラメータ
            picPen = picPen ?? pub_picPen;
            float pointCx = 0, pointCy = 0, width = 0, height = 0;

            //座標計算
            //pointData[0] 中心座標点 X
            //pointData[1] 中心座標点 Y
            //pointData[2] 半径
            //LogOutput(DateTime.Now.ToString("HH:mm") + " 画像円を描く");
            //Point_Offset はグローバル変数、
            pointCx = Convert.ToInt16(pointData[0]) - Convert.ToInt16(pointData[2]) + offset + Point_Offset.X;
            pointCy = Convert.ToInt16(pointData[1]) - Convert.ToInt16(pointData[2]) + offset + Point_Offset.Y;
            width = Convert.ToInt16(pointData[2]) * 2;
            height = Convert.ToInt16(pointData[2]) * 2;

            if (fillFlag)
            {
                SolidBrush picBru = new SolidBrush(Color.Black);
                graphObj.FillEllipse(picBru, pointCx, pointCy, width, height);
            }
            else
            {
                //picPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                graphObj.DrawEllipse(picPen, pointCx, pointCy, width, height);
            }
        }

        public void Draw_PointMode(string ObjComm, string ObjAna)
        {
            LogOutput("Draw_PointMode");
            //Define
            string[] anaData, commData;
            string backObjComm = ObjComm;
            ArrayList pointData = new ArrayList();

            //Processing
            AssignRemover(ref ObjComm);
            commData = Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase);
            anaData = Regex.Split(ObjAna, @"\|", RegexOptions.IgnoreCase);

            for (int i = 0; i < anaData.Length; i++)
            {
                if (anaData[i] == "IntNum" || anaData[i] == "DblNum")
                {
                    pointData.Add(commData[i]);
                }
            }

            //座標点の半径を手動的に
            pointData.Add(2);
            DrawCircleMode(ref pointData, true);
        }
    }
}
