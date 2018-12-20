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

            double slopeA = Math.Round((Convert.ToDouble(lineData[3]) - Convert.ToDouble(lineData[1])) / (Convert.ToDouble(lineData[2]) - Convert.ToDouble(lineData[0])), 5);
            //データの有効性判断
            IsInfinityOrNaN(ref slopeA);
            //点 C1
            double C1x = Math.Round(Convert.ToDouble(pData[0]) - slopeA * Convert.ToDouble(cirData[0]) * Math.Sqrt(1 / (Math.Pow(slopeA, 2) + 1)), 2);
            double C1y = Math.Round(Convert.ToDouble(pData[1]) + Convert.ToDouble(cirData[0]) * Math.Sqrt(1 / (Math.Pow(slopeA, 2) + 1)), 2);
            //点 C2
            double C2x = Math.Round(Convert.ToDouble(pData[0]) + slopeA * Convert.ToDouble(cirData[0]) * Math.Sqrt(1 / (Math.Pow(slopeA, 2) + 1)), 2);
            double C2y = Math.Round(Convert.ToDouble(pData[1]) - Convert.ToDouble(cirData[0]) * Math.Sqrt(1 / (Math.Pow(slopeA, 2) + 1)), 2);

            ArrayList pointData = new ArrayList();
            //Pen picPen = new Pen(Color.Black, 2.7F);
            pointData.Add(C1x); pointData.Add(C1y); pointData.Add(3);
            DrawCircleMode(ref pointData, true);

            pointData.Clear();
            pointData.Add(C2x); pointData.Add(C2y); pointData.Add(3);
            DrawCircleMode(ref pointData, true);

            //Reflush
            picBox.Refresh();
            if (!ROTATIONFLAG)
            {
                LogOutput("ROTATIONFLAG is " + ROTATIONFLAG);
                debug_Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                picBox.Refresh();
                ROTATIONFLAG = true;
            }
            MakeObjectBraille();

            LogOutput("Debug TheSolveMode Secound Touch.");
            //ここから、指先を探す機能付き
            tobeRead.SpeakAsync("もう一度触ってください。");
            Point fingerPoint = new Point(0, 0);
            double distanceCir1, distanceCir2 = 0.0;

            fingerPoint = FingerFinder();
            fingerPoint.X = fingerPoint.X - Point_Offset.X;
            fingerPoint.Y = fingerPoint.Y - Point_Offset.Y;
            LogOutput("The Secound Touch Point :  " + fingerPoint);
            //distanceCir1 = Math.Sqrt(Math.Abs(C1x - fingerPoint.X) * Math.Abs(C1x - fingerPoint.X) + Math.Abs(C1y - fingerPoint.Y) * Math.Abs(C1y - fingerPoint.Y));
            distanceCir1 = Math.Abs(C1x - fingerPoint.X);
            //distanceCir2 = Math.Sqrt(Math.Abs(C2x - fingerPoint.X) * Math.Abs(C2x - fingerPoint.X) + Math.Abs(C2y - fingerPoint.Y) * Math.Abs(C2y - fingerPoint.Y));
            distanceCir2 = Math.Abs(C2x - fingerPoint.X);
            LogOutput("");

            if (distanceCir1 > distanceCir2)
            {
                //circle 2
                fingerPoint.X = (int)C2x;
                fingerPoint.Y = (int)C2y;
            }

            else
            {
                //circle 1
                fingerPoint.X = (int)C1x;
                fingerPoint.Y = (int)C1y;
            }

            index = 0;
            foreach (var temp in lisAnaData)
            {
                if (temp != "Ident")
                    index++;
                else
                    break;
            }

            if (ObjectFinder(lisCommData[index])!= -1)
            {
                ObjCommand[ObjectFinder(lisCommData[index])] = ObjName[ObjectFinder(lisCommData[index])]+ "|=|Point|(|"+ fingerPoint.X + "|,|"+ fingerPoint.Y + "|)";
            }
            LogOutput(fingerPoint);
            LogOutput(ObjName[ObjectFinder(lisCommData[index])] + "|=|Point|(|" + fingerPoint.X + "|,|" + fingerPoint.Y + "|)");




            #endregion
        }

        /// <summary>
        /// 数字がInfinityとNaNの場合は0に変換する。
        /// </summary>
        /// <param name="num"></param>
        /// <returns>True データが0に変換した</returns>
        /// <returns>False データが変換されていない</returns>
        public bool IsInfinityOrNaN(ref double num)
        {
            if (double.IsNaN(num))
            {
                num = 0;
                return true;
            }

            if (double.IsInfinity(num))
            {
                num = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// SetPointMode相対位置機能関数
        /// </summary>
        /// <param name="objCommData">入力データ例　objA|:|setpoint|(|left|,|objB|,|right|,|10|,|0|)</param>
        /// <param name="objAnaData">入力データ例　Ident|Colon|SetPoint|Lparen|Position|Comma|Ident|Comma|Position|Comma|IntNum|Comma|IntNum|Rparen</param>
        public void TheSetPointMode(string objCommData, string objAnaData)
        {
            #region Define Part
            //objA: setpoint(left, objB, right, 10, 0)
            //objCommData  objA|:|setpoint|(|left|,|objB|,|right|,|10|,|0|)
            //命令解釈      Region:SetPoint("Point", relativeTo, "relativePoint", xOffset, yOffset)
            //objAnaData   Ident|Colon|SetPoint|Lparen|Position|Comma|Ident|Comma|Position|Comma|IntNum|Comma|IntNum|Rparen
            //Define
            int identPosition = 0;
            string identA = "", identB = "", tempComm = "";
            double objAstartX = 0, objAstartY = 0, objAendX = 0, objAendY = 0, objAlength = 0;
            double objBstartX = 0, objBstartY = 0, objBendX = 0, objBendY = 0, objBlength = 0;
            double tempWidth = 0, tempHeight = 0;
            string[] listBasedComm, listBasedAna; //Object B
            string[] listFixedComm, listFixedAna; //Object A
            ArrayList basedPointData = new ArrayList();
            ArrayList basedEdgeData = new ArrayList();
            ArrayList fixedPointData = new ArrayList();
            ArrayList fixedEdgeData = new ArrayList();
            ArrayList redrawPointData = new ArrayList();
            List<string> positionData = new List<string>();
            List<string> targetIdent = new List<string>();
            List<double> objOffset = new List<double>();
            string[] listCommData = Regex.Split(objCommData, @"\|", RegexOptions.IgnoreCase);
            string[] listAnaData = Regex.Split(objAnaData, @"\|", RegexOptions.IgnoreCase);

            LogOutput("Processing in TheSetPointMode Function");
            #endregion

            #region 事前に入力データを検査する
            //string finder in listAnaData
            for (int i = 0; i < listAnaData.Length; i++)
            {
                if (listAnaData[i] == "Ident")
                {
                    if (ObjectFinder(listCommData[i]) != -1)
                    {
                        targetIdent.Add(listCommData[i]);
                    }
                }

                if (listAnaData[i] == "Position")
                {
                    positionData.Add(listCommData[i].ToLower());
                }

                if (listAnaData[i] == "IntNum" || listAnaData[i] == "DblNum")
                {
                    objOffset.Add(Convert.ToDouble(listCommData[i]));
                }
            }

            if (targetIdent.Count != 2 || positionData.Count != 2)
            {
                LogOutput("TheSetPointMode相対位置関数パラメーターミス、処理中止。");
                tobeRead.SpeakAsync("TheSetPointMode相対位置関数パラメーターミス、処理中止。もう一度入力してください！");
                return;
            }
            #endregion

            #region Object B の輪郭を計算する
            identPosition = ObjectFinder(targetIdent[1]);
            tempComm = ObjCommand[identPosition].ToString();
            AssignRemover(ref tempComm);
            listBasedComm = Regex.Split(tempComm, @"\|", RegexOptions.IgnoreCase);
            listBasedAna = Regex.Split(ObjAnalysis[identPosition].ToString(), @"\|", RegexOptions.IgnoreCase);
            //描画点を取る
            for (int i = 0; i < listBasedAna.Length; i++)
            {
                if (listBasedAna[i] == "IntNum" || listBasedAna[i] == "DblNum")
                {
                    basedPointData.Add(listBasedComm[i]);
                }
            }

            //輪郭を計算する
            identB = listBasedAna[0];
            switch (identB)
            {
                case "Line":
                    #region Line Case
                    if (basedPointData.Count == 4)
                    {
                        //AnchorPoint 左上
                        //basedPointData[0]  始点x -> objBstartX
                        //basedPointData[1]  始点y -> objBstartY
                        //basedPointData[2]  終点x -> objBendX
                        //basedPointData[3]  終点y -> objBendY
                        objBstartX = Convert.ToDouble(basedPointData[0]);
                        objBstartY = Convert.ToDouble(basedPointData[1]);
                        objBendX = Convert.ToDouble(basedPointData[2]);
                        objBendY = Convert.ToDouble(basedPointData[3]);

                        if (objBstartX < objBendX && objBstartY < objBendY)
                        {
                            basedEdgeData.Add(objBstartX);
                            basedEdgeData.Add(objBendY);
                            basedEdgeData.Add(objBendX - objBstartX);
                            basedEdgeData.Add(objBendY - objBstartY);
                        }
                        else if (objBstartX < objBendX && objBstartY > objBendY)
                        {
                            basedEdgeData.Add(objBstartX);
                            basedEdgeData.Add(objBstartY);
                            basedEdgeData.Add(objBendX - objBstartX);
                            basedEdgeData.Add(objBstartY - objBendY);
                        }
                        else if (objBstartX < objBendX && objBstartY == objBendY)
                        {
                            //水平線
                            basedEdgeData.Add(objBstartX);
                            basedEdgeData.Add(objBstartY + 3);
                            basedEdgeData.Add(objBendX - objBstartX);
                            basedEdgeData.Add(5);
                        }
                        else if (objBstartX == objBendX && objBstartY < objBendY)
                        {
                            //垂直線
                            basedEdgeData.Add(objBendX - 3);
                            basedEdgeData.Add(objBendY);
                            basedEdgeData.Add(5);
                            basedEdgeData.Add(objBendY - objBstartY);
                        }
                        else if (objBendX < objBstartX && objBendY < objBstartY)
                        {
                            //AnchorPoint 左上
                            basedEdgeData.Add(objBendX);
                            basedEdgeData.Add(objBstartY);
                            basedEdgeData.Add(objBstartX - objBendX);
                            basedEdgeData.Add(objBstartY - objBendY);
                        }
                        else if (objBendX < objBstartX && objBendY > objBstartY)
                        {
                            //AnchorPoint 左上
                            basedEdgeData.Add(objBendX);
                            basedEdgeData.Add(objBstartY);
                            basedEdgeData.Add(objBstartX - objBendX);
                            basedEdgeData.Add(objBstartY - objBendY);
                        }
                        else
                        {
                            LogOutput("線処理未知条件");
                        }
                    }
                    else if (basedPointData.Count == 5)
                    {
                        objBstartX = Convert.ToDouble(basedPointData[0]);
                        objBstartY = Convert.ToDouble(basedPointData[1]);
                        objBendX = Convert.ToDouble(basedPointData[2]);
                        objBendY = Convert.ToDouble(basedPointData[3]);
                        objBlength = Convert.ToDouble(basedPointData[4]);
                    }
                    else
                    {
                        LogOutput("パラメーターミス");
                        return;
                    }
                    #endregion
                    break;
                case "Circle":
                    #region Circle Case
                    if (basedPointData.Count == 1)
                    {
                        //Anchor Point 左上
                        //basedPointData[0]  半径r
                        //左上座標点 -> objBstartX, objBstartY
                        //右下座標点 -> objBendX, objBendY
                        //Define
                        int index;
                        string subIdent, subComm, subAna;
                        string[] subCommData, subChkData;
                        ArrayList subPointData = new ArrayList();
                        //Processing
                        //chkData   "["circle", "(", "c", ",", "5", ")"]"
                        index = IdentFinder(ref listBasedAna);
                        if (index == -1)
                        {
                            tobeRead.SpeakAsync("円定義の中にミスが存在している、もう一度確認してください。");
                            return;
                        }
                        subIdent = listBasedComm[index];
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
                        subChkData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);
                        for (int i = 0; i < subChkData.Length; i++)
                        {
                            if (subChkData[i] == "IntNum" || subChkData[i] == "DblNum")
                            {
                                subPointData.Add(subCommData[i]);
                            }
                        }
                        if (subCommData[0].ToLower() == "point" && subChkData[0] == "Point" && subPointData.Count == 2)
                        {
                            subPointData.Add(basedPointData[basedPointData.Count - 1]);
                            objBstartX = Convert.ToDouble(subPointData[0]) - Convert.ToDouble(subPointData[2]);
                            objBstartY = Convert.ToDouble(subPointData[1]) - Convert.ToDouble(subPointData[2]);
                            objBendX = Convert.ToDouble(subPointData[0]) + Convert.ToDouble(subPointData[2]);
                            objBendY = Convert.ToDouble(subPointData[1]) + Convert.ToDouble(subPointData[2]);
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
                    else if (basedPointData.Count == 3)
                    {
                        //Anchor Point 左上
                        //basedPointData[0]  中心座標点x
                        //basedPointData[1]  中心座標点y
                        //basedPointData[2]  半径r
                        //左上座標点 -> objBstartX, objBstartY
                        //右下座標点 -> objBendX, objBendY
                        objBstartX = Convert.ToDouble(basedPointData[0]) - Convert.ToDouble(basedPointData[2]);
                        objBstartY = Convert.ToDouble(basedPointData[1]) - Convert.ToDouble(basedPointData[2]);
                        objBendX = Convert.ToDouble(basedPointData[0]) + Convert.ToDouble(basedPointData[2]);
                        objBendY = Convert.ToDouble(basedPointData[1]) + Convert.ToDouble(basedPointData[2]);
                    }
                    else
                    {
                        LogOutput("円処理未知条件");
                    }
                    #endregion
                    break;
                default:
                    codeOutput("Error Unsupported Ident " + identB + " @TheSetPointMode @switch 対象B処理.");
                    break;
            }
            #endregion

            #region Object A の輪郭を計算する
            basedPointData.Clear();
            identPosition = ObjectFinder(targetIdent[0]);
            tempComm = ObjCommand[identPosition].ToString();
            AssignRemover(ref tempComm);
            listFixedComm = Regex.Split(tempComm, @"\|", RegexOptions.IgnoreCase);
            listFixedAna = Regex.Split(ObjAnalysis[identPosition].ToString(), @"\|", RegexOptions.IgnoreCase);
            //描画点を取る
            for (int i = 0; i < listFixedAna.Length; i++)
            {
                if (listFixedAna[i] == "IntNum" || listFixedAna[i] == "DblNum")
                {
                    basedPointData.Add(listFixedComm[i]);
                }
            }

            //輪郭を計算する
            identA = listFixedAna[0];
            switch (identA)
            {
                case "Line":

                    break;
                case "Circle":
                    #region Circle Case
                    if (basedPointData.Count == 1)
                    {
                        //Anchor Point 左上
                        //basedPointData[0]  半径r
                        //左上座標点 -> objAstartX, objAstartY
                        //右下座標点 -> objAendX, objAendY
                        //Define
                        int index;
                        string subIdent, subComm, subAna;
                        string[] subCommData, subChkData;
                        ArrayList subPointData = new ArrayList();
                        //Processing
                        //chkData   "["circle", "(", "c", ",", "5", ")"]"
                        index = IdentFinder(ref listFixedAna);
                        if (index == -1)
                        {
                            tobeRead.SpeakAsync("円定義の中にミスが存在している、もう一度確認してください。");
                            return;
                        }

                        subIdent = listFixedComm[index];
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
                        subChkData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                        for (int i = 0; i < subChkData.Length; i++)
                        {
                            if (subChkData[i] == "IntNum" || subChkData[i] == "DblNum")
                            {
                                subPointData.Add(subCommData[i]);
                            }
                        }

                        if (subCommData[0].ToLower() == "point" && subChkData[0] == "Point" && subPointData.Count == 2)
                        {
                            subPointData.Add(basedPointData[basedPointData.Count - 1]);
                            objAstartX = Convert.ToDouble(subPointData[0]) - Convert.ToDouble(subPointData[2]);
                            objAstartY = Convert.ToDouble(subPointData[1]) - Convert.ToDouble(subPointData[2]);
                            objAendX = Convert.ToDouble(subPointData[0]) + Convert.ToDouble(subPointData[2]);
                            objAendY = Convert.ToDouble(subPointData[1]) + Convert.ToDouble(subPointData[2]);
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

                    else if (basedPointData.Count == 3)
                    {
                        //Anchor Point 左上
                        //basedPointData[0]  中心座標点x
                        //basedPointData[1]  中心座標点y
                        //basedPointData[2]  半径r
                        //左上座標点 -> objAstartX, objAstartY
                        //右下座標点 -> objAendX, objAendY
                        objAstartX = Convert.ToDouble(basedPointData[0]) - Convert.ToDouble(basedPointData[2]);
                        objAstartY = Convert.ToDouble(basedPointData[1]) - Convert.ToDouble(basedPointData[2]);
                        objAendX = Convert.ToDouble(basedPointData[0]) + Convert.ToDouble(basedPointData[2]);
                        objAendY = Convert.ToDouble(basedPointData[1]) + Convert.ToDouble(basedPointData[2]);
                    }

                    else
                    {
                        LogOutput("円処理未知条件");
                    }
                    #endregion
                    break;
                default:
                    codeOutput("Error Unsupported Ident " + identA + "  @TheSetPointMode @switch 対象A処理.");
                    break;
            }
            #endregion

            #region Object A の輪郭位置を再計算する
            //相対位置パラメーター　positionData[0] -> left を基準になる
            //targetIdent[1] -> objB
            //相対位置パラメーター　positionData[1] -> right
            //targetIdent[0] -> objA
            //objOffset[0] -> xOffset   objOffset[1] -> yOffset
            tempWidth = objAendX - objAstartX;
            tempHeight = objAendY - objAstartY;
            //debug information
            LogOutput(targetIdent[1] + "座標データ >>" + "objBstartX: " + objBstartX + "/objBstartY: " + objBstartY + "/objBendX: " + objBendX + "/objBendY: " + objBendY);
            LogOutput(targetIdent[0] + "座標データ >>" + "objAstartX: " + objAstartX + "/objAstartY: " + objAstartY + "/objAendX: " + objAendX + "/objAendY: " + objAendY);
            LogOutput("tempWidth: "+ tempWidth + "   tempHeight: " + tempHeight);

            if (positionData[0].ToLower() == "left" && positionData[1].ToLower() == "right")
            {
                LogOutput(targetIdent[1] + "左 -> " + targetIdent[0] + "右");
                //objB -> left   objA -> right
                objAstartX = objBendX + objOffset[0];
                objAstartY = objBstartY + objOffset[1];
                objAendX = objAstartX + tempWidth;
                objAendY = objAstartY + tempHeight;
            }
            
            else if (positionData[0].ToLower() == "bottomright" && positionData[1].ToLower() == "topleft")
            {
                LogOutput(targetIdent[1] + "右下 -> " + targetIdent[0] + "左上");
                //objB -> bottomright   objA -> topleft
                objAendX = objBstartX - objOffset[0];
                objAendY = objBstartY - objOffset[1];
                objAstartX = objAendX - tempWidth;
                objAstartY = objAendY - tempHeight;
            }
            
            else if (positionData[0].ToLower() == "topright" && positionData[1].ToLower() == "bottomleft")
            {
                LogOutput(targetIdent[1] + "右上 -> " + targetIdent[0] + "左下");
                //objB -> topright   objA -> bottomleft
                objAendX = objBstartX - objOffset[0];
                objAendY = objBendY + tempHeight + objOffset[1];
                objAstartX = objAendX - tempWidth;
                objAstartY = objAendY - tempHeight;
            }
            
            else if (positionData[0].ToLower() == "right" && positionData[1].ToLower() == "left")
            {
                LogOutput(targetIdent[1] + "右 -> " + targetIdent[0] + "左");
                //objB -> right   objA -> left
                objAendX = objBstartX - objOffset[0];
                objAendY = objBstartY + tempHeight + objOffset[1];
                objAstartX = objAendX - tempWidth;
                objAstartY = objAendY - tempHeight;
            }
            
            else if (positionData[0].ToLower() == "bottomleft" && positionData[1].ToLower() == "topright")
            {
                LogOutput(targetIdent[1] + "左下 -> " + targetIdent[0] + "右上");
                //objB -> bottomleft   objA -> topright
                objAstartX = objBendX + objOffset[0];
                objAstartY = objBstartY - tempHeight - objOffset[1];
                objAendX = objAstartX + tempWidth;
                objAendY = objAstartY + tempHeight;
            }
            
            else if (positionData[0].ToLower() == "topleft" && positionData[1].ToLower() == "bottomright")
            {
                LogOutput(targetIdent[1] + "左上 -> " + targetIdent[0] + "右下");
                //objB -> topleft   objA -> bottomright
                objAstartX = objBendX + objOffset[0];
                objAstartY = objBendY + objOffset[1];
                objAendX = objAstartX + tempWidth;
                objAendY = objAstartY + tempHeight;
            }
            
            else if (positionData[0].ToLower() == "top" && positionData[1].ToLower() == "bottom")
            {
                LogOutput(targetIdent[1] + "上 -> " + targetIdent[0] + "下");
                //objB -> top   objA -> bottom
                objAstartX = objBstartX + objOffset[0];
                objAstartY = objBendY + objOffset[1];
                objAendX = objAstartX + tempWidth;
                objAendY = objAstartY + tempHeight;
            }
            
            else if (positionData[0].ToLower() == "bottom" && positionData[1].ToLower() == "top")
            {
                LogOutput(targetIdent[1] + "下 -> " + targetIdent[0] + "上");
                //objB -> bottom   objA -> top
                objAstartX = objBstartX - objOffset[0];
                objAstartY = objBstartY - tempHeight - objOffset[1];
                objAendX = objAstartX + tempWidth;
                objAendY = objAstartY + tempHeight;
            }

            else
            {
                LogOutput("入力した相対位置条件はサポートされていません。");
            }

            LogOutput(targetIdent[0] + "再計算結果" + "/objAstartX: " + objAstartX + "/objAstartY: " + objAstartY + "/objAendX: " + objAendX + "/objAendY: " + objAendY);
            #endregion

            #region Object A を再描画する
            LogOutput("Object A を再描画する");
            switch (identA)
            {
                case "Line":

                    break;
                case "Circle":
                    #region Circle Case
                    if (tempHeight / 2 != tempWidth / 2)
                    {
                        return;
                    }
                    redrawPointData.Add(objAstartX + (tempWidth / 2));
                    redrawPointData.Add(objAstartY + (tempHeight / 2));
                    redrawPointData.Add(tempHeight / 2);
                    DrawCircleMode(ref redrawPointData, pub_picPen);

                    //Rewrite the Data
                    identPosition = ObjectFinder(targetIdent[0]);
                    //円形の命令データを書き直し
                    ObjCommand.RemoveAt(identPosition);
                    ObjCommand.Insert(identPosition, targetIdent[0] + "|=|" + identA + "|(|" + redrawPointData[0] + "|,|" + redrawPointData[1] + "|,|" + redrawPointData[2] + "|)");
                    //円形の解析データを書き直し
                    ObjAnalysis.RemoveAt(identPosition);
                    ObjAnalysis.Insert(identPosition, "Circle|Lparen|IntNum|Comma|IntNum|Comma|IntNum|Rparen");
                    #endregion
                    break;
                default:
                    codeOutput("Error Unsupported Ident " + identA + "  @TheSetPointMode @switch 対象A処理.");
                    break;
            }
            #endregion
            picBox.Refresh();
            MakeObjectBraille();
        }
    }
}
