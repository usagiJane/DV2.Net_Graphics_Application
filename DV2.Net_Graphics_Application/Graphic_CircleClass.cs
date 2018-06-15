using System;
using System.Collections.Generic;
using System.Text;
#region Append
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
            LogOutput("Draw_CircleMode");
            //Define
            string[] chkData, commData;
            ArrayList pointData = new ArrayList();
            Pen picPen = new Pen(Color.Black, 2.7F);

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

            if(pointData.Count == 1)
            {
                //Define
                int index;
                string subIdent, subComm, subAna;
                string[] subCommData, subChkData;
                ArrayList subPointData = new ArrayList();

                //Processing
                index = IdentFinder(ref chkData);
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

                subComm = ObjCommand[index].ToString();
                subAna = ObjAnalysis[index].ToString();
                AssignRemover(ref subComm);
                subCommData = Regex.Split(subComm, @"\|", RegexOptions.IgnoreCase);
                subChkData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                for (int i = 0; i < subChkData.Length; i++)
                {
                    if (subChkData[i] == "IntNum" || subChkData[i] == "DblNum")
                    {
                        subPointData.Add(subCommData[i]);
                    }
                }

                if(subCommData[0].ToLower() == "point" && subChkData[0] == "Point" && subPointData.Count == 2)
                {
                    
                    if (commData[0].ToLower() == "circle" && chkData[0] == "Circle")
                    {
                        subPointData.Add(pointData[pointData.Count - 1]);
                        DrawCircleMode(ref subPointData, picPen);
                    }
                    else if (commData[0].ToLower() == "fillcircle" && chkData[0] == "FillCircle")
                    {
                        subPointData.Add(pointData[pointData.Count - 1]);
                        DrawCircleMode(ref subPointData, picPen, true);
                    }
                }
                else
                {
                    tobeRead.SpeakAsync("Error @Draw_CircleMode");
                }

            }

            else if (commData[0].ToLower() == "circle" && chkData[0] == "Circle" && pointData.Count == 3)
            {
                DrawCircleMode(ref pointData, picPen);
            }
            else if (commData[0].ToLower() == "fillcircle" && chkData[0] == "FillCircle" && pointData.Count == 3)
            {
                DrawCircleMode(ref pointData, picPen, true);
            }
            else
            {
                tobeRead.SpeakAsync("@Graphic_CircleClass Draw_CircleMode関数" + ObjName[ObjCommand.BinarySearch(ObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_CircleClass Draw_CircleMode関数" + ObjName[ObjCommand.BinarySearch(ObjComm)] + "対象定義識別失敗!");
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
            pointCx = Convert.ToSingle(pointData[0]) - Convert.ToSingle(pointData[2]) + offset;
            pointCy = Convert.ToSingle(pointData[1]) + Convert.ToSingle(pointData[2]) + offset;
            width = Convert.ToSingle(pointData[2]) * 2 + offset;
            height = Convert.ToSingle(pointData[2]) * 2 + offset;

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
    }
}
