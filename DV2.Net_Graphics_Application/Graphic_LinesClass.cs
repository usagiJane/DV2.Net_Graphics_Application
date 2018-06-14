using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;

namespace DV2.Net_Graphics_Application
{
    //class Graphic_LinesClass
    public partial class MainForm
    {
        public void drawLine()
        {
            //LogOutput("画像ライン");
            ////Point stratPoint = new Point(0, 0);
            ////Point endPoint = new Point(0, 0);
            //int pointX1 = 0, pointY1 = 0, pointX2 = 0, pointY2 = 0;
            //string point1, point2;
            //string[] point11, point22;

            //point1 = textBox2.Text;
            //point2 = textBox3.Text;
            //point11 = point1.Split(',');
            //point22 = point2.Split(',');
            //int.TryParse(point11[0], out pointX1);
            //int.TryParse(point11[1], out pointY1);
            //int.TryParse(point22[0], out pointX2);
            //int.TryParse(point22[1], out pointY2);
            ////stratPoint.X = poiX1;

            ////ラインを書く
            //graphObj.DrawLine(picPen, pointX1, pointY1, pointX2, pointY2);

            //graphObj.DrawLine(picPen, int.Parse(txtBoxSubdata[1]) + offset, int.Parse(txtBoxSubdata[2]) + offset, int.Parse(txtBoxSubdata[3]) + offset, int.Parse(txtBoxSubdata[4]) + offset);
        }

        public void Draw_LineMode(string ObjComm, string ObjAna)
        {
            //Debug
            LogOutput("\r\n@Graphic_LinesClass  -> Draw_LineMode");
            LogOutput(ObjCommand);
            LogOutput(ObjAnalysis + "\r\n");
            //System.Windows.Forms.MessageBox.Show("Draw_LineMode");
            //Define
            string[] chkData, tempData;
            ArrayList commData = new ArrayList();
            tempData = Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase);
            chkData = Regex.Split(ObjAna, @"\|", RegexOptions.IgnoreCase);
            
            for (int i = 0; i < chkData.Length; i++)
            {
                if (chkData[i] == "IntNum" || chkData[i] == "DblNum")
                {
                    commData.Add(tempData[i]);
                }
            }

            switch (commData.Count)
            {
                case 4:
                    Draw_LineMode(commData,);
                    break;
                default:
                    {
                        tobeRead.Speak("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjCommand.BinarySearch(ObjComm)] + "対象定義識別失敗!");
                        codeOutput("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjCommand.BinarySearch(ObjComm)] + "対象定義識別失敗!");
                        break;
                    }
            }
        }

        private void Draw_LineMode(ref ArrayList commData, ref Pen picPen, float offset = pub_offSet)
        {
            //graphObj 
            //picPen
            //offset 変位量
            //commData 命令文内容,可変パラメータ
            float pointAx = 0, pointAy = 0, pointBx = 0, pointBy = 0;

            //座標計算
            pointAx = Convert.ToSingle(commData[1]) + offset;
            pointAy = Convert.ToSingle(commData[2]) + offset;
            pointBx = Convert.ToSingle(commData[3]) + offset;
            pointBy = Convert.ToSingle(commData[4]) + offset;
            
            graphObj.DrawLine(picPen, pointAx, pointAy, pointBx, pointBy);
        }

        private void Draw_LineMode(ref ArrayList commData, Pen picPen = null, float offset = pub_offSet )
        {
            //graphObj 
            //picPen
            picPen = picPen ?? pub_picPen;
            //offset 変位量
            //commData 命令文内容,可変パラメータ
            float pointAx = 0, pointAy = 0, pointBx = 0, pointBy = 0;

            if (commData.Length == 5)
            {
                //座標計算
                pointAx = Convert.ToSingle(commData[1]) + offset;
                pointAy = Convert.ToSingle(commData[2]) + offset;
                pointBx = Convert.ToSingle(commData[3]) + offset;
                pointBy = Convert.ToSingle(commData[4]) + offset;
            }


            graphObj.DrawLine(picPen, pointAx, pointAy, pointBx, pointBy);
        }

        public void drawArrow()
        {
            //LogOutput("矢印");
            //int pointX1 = 0, pointY1 = 0, pointX2 = 0, pointY2 = 0;
            //string point1, point2;
            //string[] point11, point22;

            //point1 = textBox2.Text;
            //point2 = textBox3.Text;
            //point11 = point1.Split(',');
            //point22 = point2.Split(',');
            //int.TryParse(point11[0], out pointX1);
            //int.TryParse(point11[1], out pointY1);
            //int.TryParse(point22[0], out pointX2);
            //int.TryParse(point22[1], out pointY2);
            ////picPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            //picPen.EndCap = System.Drawing.Drawing2D.LineCap.Triangle;

            ////矢印を書く
            //graphObj.DrawLine(picPen, pointX1, pointY1, pointX2, pointY2);
        }

        public void Draw_ArrowMode(string ObjCommand, string ObjAnalysis)
        {
            LogOutput("@Graphic_LinesClass -> Draw_ArrowMode");
            
        }

        private void Draw_ArrowMode(ref string[] txtBoxSubdata, ref Pen picPen, float offset = pub_offSet)
        {

        }

        private void Draw_ArrowMode(ref string[] txtBoxSubdata, Pen picPen = null, float offset = pub_offSet)
        {
            //graphObj 
            //picPen
            picPen = picPen ?? pub_picPen;
            //offset 変位量
            //txtBoxSubdata 命令文内容,可変パラメータ

        }
    }
}
