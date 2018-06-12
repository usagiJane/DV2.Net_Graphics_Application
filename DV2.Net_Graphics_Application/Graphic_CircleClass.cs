using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace DV2.Net_Graphics_Application
{
    //class Graphic_CircleClass
    public partial class DV2_Drawing
    {
        //
        public void Draw_CircleMode(string ObjCommand, string ObjAnalysis, MainForm mLog)
        {
            if (mfLog == null)
            {
                mfLog = mLog;
            }
            LogOutput("Draw_CircleMode");

            string[] commData;

        }
        
        private void Draw_CircleMode(Graphics graphObj, ref string[] commData, ref Pen picPen, float offset = pub_offSet)
        {
            //graphObj 
            //picPen
            //offset 変位量
            //commData 命令文内容,可変パラメータ
            float pointAx = 0, pointAy = 0, width = 0, height = 0;

            //座標計算
            //LogOutput(DateTime.Now.ToString("HH:mm") + " 画像円を描く");
            pointAx = Convert.ToSingle(commData[1]) + offset;
            pointAy = Convert.ToSingle(commData[2]) + offset;
            width = Convert.ToSingle(commData[3]) + offset;
            height = Convert.ToSingle(commData[4]) + offset;
            
            graphObj.DrawEllipse(picPen, pointAx, pointAy, width, height);
        }
    }
}
