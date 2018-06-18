using System;

#region Append
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
            //System.Windows.Forms.MessageBox.Show("Draw_LineMode");

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
                tobeRead.SpeakAsync("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjCommand.BinarySearch(ObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjCommand.BinarySearch(ObjComm)] + "対象定義識別失敗!");
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
                picPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
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

            if (commData[0].ToLower() == "line" && chkData[0] == "Line" && pointData.Count == 4)
            {
                DrawArrow(ref pointData, picPen);
            }
            else if (commData[0].ToLower() == "dasharrow" && chkData[0] == "DashArrow" && pointData.Count == 4)
            {
                DrawArrow(ref pointData, picPen, true);
            }
            else
            {
                tobeRead.SpeakAsync("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjCommand.BinarySearch(ObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjCommand.BinarySearch(ObjComm)] + "対象定義識別失敗!");
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
                picPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            }
            picPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            graphObj.DrawLine(picPen, pointAx, pointAy, pointBx, pointBy);
        }
    }
}
