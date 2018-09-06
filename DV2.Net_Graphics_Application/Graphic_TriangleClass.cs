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
    //class Graphic_TriangleClass　三角形
    public partial class MainForm
    {
        public void Draw_TriangleMode(string ObjComm, string ObjAna)
        {
            //Debug
            LogOutput("Draw_QuadrilateralMode");
            LogOutput("Parameter 1  -> " + ObjComm);
            LogOutput("Parameter 2  -> " + ObjAna + "\r\n");

            //Define
            string[] chkData, commData;
            string backObjComm = ObjComm;
            ArrayList pointData = new ArrayList();
            Pen picPen = new Pen(Color.Black, 1F);

            //Processing
            AssignRemover(ref ObjComm);
            commData = Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase);
            chkData = Regex.Split(ObjAna, @"\|", RegexOptions.IgnoreCase);

        }

        private void DrawTriangle(ref ArrayList pointData, Pen picPen = null, bool dashFlag = false, float offset = pub_offSet)
        {
            LogOutput("DrawTriangle (" + pointData[0] + "," + pointData[1] + ") -> (" + pointData[2] + "," + pointData[3] + ") -> (" + pointData[2] + "," + pointData[3] + ")");
        }
    }
}
