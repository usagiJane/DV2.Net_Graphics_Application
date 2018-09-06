using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Drawing;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        public void TheSolveMode(string objCommData, string[] lisCommData, string objAnaData, string[] lisAnaData)
        {
            //solve c by contact(obj1,ojb2,p)
            //objCommData	"solve|c|by|contact|(|obj1|,|ojb2|,|p|)"
            //objAnaData	"Solve|Ident|Ident|Contact|Lparen|Ident|Comma|Ident|Comma|Ident|Rparen"
            //obj1          既知対象、未知変数など一切なし。
            //obj2          未知対象、未知変数が存在する。
            //Define
            int index = 0;
            string tempComm;
            string[] unknownComm, unknownAna;
            List<string> targetIdent = new List<string>();

            //Processing
            foreach (var temp in lisAnaData)
            {
                if (temp != "Contact")
                    index++;
                else
                    break;
            }

            unknownComm = lisCommData.Skip(index + 1).ToArray();
            unknownAna = lisAnaData.Skip(index + 1).ToArray();
            index = 0;

            foreach (var temp in unknownAna)
            {
                if (temp == "Ident")
                {
                    targetIdent.Add(unknownComm[index]);
                }
                index++;
            }

            //再修正必要
            //Case by case
            if (targetIdent.Count == 3)
            {
                try
                {
                    //Contents 1
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                    codeOutput(ex.Message);
                }
            }

            #region Contents 1
            //line   aを計算する
            //cir   求める
            //p    円の中心座標
            string lineObj = targetIdent[0];
            string cirObj = targetIdent[1];
            string pCont = targetIdent[2];

            int lineIdx = ObjectFinder(lineObj);
            if (lineIdx < 0)
            {
                codeOutput("Error @solvecontact.cs  TheSolveMode関数");
                return;
            }
            int cirIdx = ObjectFinder(cirObj);
            if (cirIdx < 0)
            {
                codeOutput("Error @solvecontact.cs  TheSolveMode関数");
                return;
            }
            int pIdx = ObjectFinder(pCont);
            if (pIdx < 0)
            {
                codeOutput("Error @solvecontact.cs  TheSolveMode関数");
                return;
            }

            ArrayList lineData = new ArrayList();
            ArrayList cirData = new ArrayList();
            ArrayList pData = new ArrayList();

            //Line
            //line(1,2,20.0,25.5)
            tempComm = ObjCommand[lineIdx].ToString();
            AssignRemover(ref tempComm);
            index = 0;
            foreach (var temp in Regex.Split(ObjAnalysis[lineIdx].ToString(), @"\|", RegexOptions.IgnoreCase))
            {
                if (temp == "IntNum" || temp == "DblNum")
                {
                    lineData.Add(tempComm.Split('|')[index]);
                }
                index++;
            }

            //Circle
            //circle(c,20.0)
            tempComm = ObjCommand[cirIdx].ToString();
            AssignRemover(ref tempComm);
            index = 0;
            foreach (var temp in Regex.Split(ObjAnalysis[cirIdx].ToString(), @"\|", RegexOptions.IgnoreCase))
            {
                if (temp == "IntNum" || temp == "DblNum")
                {
                    cirData.Add(tempComm.Split('|')[index]);
                }
                index++;
            }

            //Point P
            //p(30,20)
            tempComm = ObjCommand[pIdx].ToString();
            AssignRemover(ref tempComm);
            index = 0;
            foreach (var temp in Regex.Split(ObjAnalysis[pIdx].ToString(), @"\|", RegexOptions.IgnoreCase))
            {
                if (temp == "IntNum" || temp == "DblNum")
                {
                    pData.Add(tempComm.Split('|')[index]);
                }
                index++;
            }

            double slopeA = Math.Round((Convert.ToDouble(lineData[3]) - Convert.ToDouble(lineData[1])) / (Convert.ToDouble(lineData[2]) - Convert.ToDouble(lineData[0])));
            //点 C1
            double C1x = Math.Round(Convert.ToDouble(pData[0]) - slopeA * Convert.ToDouble(cirData[0]) * Math.Sqrt(1 / (Math.Pow(slopeA, 2) + 1)));
            double C1y = Math.Round(Convert.ToDouble(pData[1]) + Convert.ToDouble(cirData[0]) * Math.Sqrt(1 / (Math.Pow(slopeA, 2) + 1)));
            //点 C2
            double C2x = Math.Round(Convert.ToDouble(pData[0]) + slopeA * Convert.ToDouble(cirData[0]) * Math.Sqrt(1 / (Math.Pow(slopeA, 2) + 1)));
            double C2y = Math.Round(Convert.ToDouble(pData[1]) - Convert.ToDouble(cirData[0]) * Math.Sqrt(1 / (Math.Pow(slopeA, 2) + 1)));

            ArrayList pointData = new ArrayList();
            //Pen picPen = new Pen(Color.Black, 2.7F);
            pointData.Add(C1x); pointData.Add(C1y); pointData.Add(3);
            DrawCircleMode(ref pointData, true);

            pointData.Clear();
            pointData.Add(C2x); pointData.Add(C2y); pointData.Add(3);
            DrawCircleMode(ref pointData, true);

            LogOutput("Debug");
            //ここから、指先を探す機能付き
            /* 20180717 Need Fix!
            Point fingerPoint = new Point(0, 0);
            string targetName = "";
            fingerPoint = FingerFinder(targetName);
            PointOnObject(ref fingerPoint, targetName);
            */
            #endregion
        }

    }
}
