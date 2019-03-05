using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;
#endregion

namespace DV2.Net_Graphics_Application
{
    //class Graphic_CurveClass 曲線
    public partial class MainForm
    {
        /// <summary>
        /// 曲線を描く関数の入口，命令文と解析データを入力すれば，曲線を描く
        /// </summary>
        /// <param name="ObjComm"></param>
        /// <param name="ObjAna"></param>
        public void Draw_CurveMode(string ObjComm, string ObjAna)
        {
            //Debug
            LogOutput("Draw_CurveMode");
            LogOutput("Parameter 1  -> " + ObjComm);
            LogOutput("Parameter 2  -> " + ObjAna + "\r\n");

            //Define
            string[] chkData, commData;
            string backObjComm = ObjComm;
            ArrayList pointData = new ArrayList();
            Pen picPen = new Pen(Color.Black, 0.1F);

            //Processing
            AssignRemover(ref ObjComm);
            commData = Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase);
            chkData = Regex.Split(ObjAna, @"\|", RegexOptions.IgnoreCase);

        }

        /// <summary>
        /// 実際に曲線を描く関数
        /// </summary>
        /// <param name="pointData"></param>
        /// <param name="picPen"></param>
        /// <param name="dashFlag"></param>
        /// <param name="offset"></param>
        private void DrawCurve(ref ArrayList pointData, Pen picPen = null, bool dashFlag = false, float offset = pub_offSet)
        {
            LogOutput("DrawCurve (" + pointData[0] + "," + pointData[1] + ") -> (" + pointData[2] + "," + pointData[3] + ")");
        }
    }
}
