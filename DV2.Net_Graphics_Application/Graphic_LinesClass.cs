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
        /// <summary>
        /// 直線を描く関数の入口，命令文と解析データを入力すれば，直線を描く
        /// </summary>
        /// <param name="ObjComm">命令文</param>
        /// <param name="ObjAna">解析データ</param>
        public void Draw_LineMode(string ObjComm, string ObjAna)
        {
            //Debug
            //LogOutput("\r\n@Graphic_LinesClass  -> Draw_LineMode");
            //LogOutput("Parameter 1  -> " + ObjComm);
            //LogOutput("Parameter 2  -> " + ObjAna + "\r\n");
            //Parameter 1->obj1 |=| line | (| 0 |,| 5 |,| 200.0 |,| 405.0 |)
            //Parameter 2->Line | Lparen | IntNum | Comma | IntNum | Comma | DblNum | Comma | DblNum | Rparen

            //Define
            #region 伸ばすモード 用パラメーター
            double distance = 0.0;
            double unKnownX = 0, unKnownY = 0;
            double slopeA = 0, InterceptB = 0, alpha = 0;
            #endregion
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

            #region pointData.Count == 0
            //例: j = line(A, B)[11]
            if (pointData.Count == 0)
            {
                //subIdent[0] - Ident対象名
                //subIdent[1] - Ident対象名
                //subIdent[2] - Ident対象の「ObjName」配列中の位置
                //subIdent[3] - Ident対象の「ObjName」配列中の位置
                int looptime = 0;
                string subComm, subAna;
                string[] subCommData, subAnaData;
                ArrayList subIdent = new ArrayList();
                ArrayList subPointData = new ArrayList();

                for (int i = 0; i < anaData.Length; i++)
                {
                    if (anaData[i] == "Ident")
                    {
                        subIdent.Add(commData[i]);
                    }
                }

                looptime = subIdent.Count;

                if (ObjectFinder(subIdent[0].ToString()) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent[0] + "Error");
                    return;
                }

                if (ObjectFinder(subIdent[1].ToString()) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent[1] + "Error");
                    return;
                }

                subIdent.Add(ObjectFinder(subIdent[0].ToString()));
                subIdent.Add(ObjectFinder(subIdent[1].ToString()));

                for (int loopi = 0; loopi < looptime; loopi++)
                {
                    subComm = ObjCommand[Convert.ToInt32(subIdent[loopi + 2])].ToString();
                    subAna = ObjAnalysis[Convert.ToInt32(subIdent[loopi + 2])].ToString();
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
                }

                //Check The Data Length
                if (subPointData.Count != 4)
                {
                    return;
                }

                if (commData[0].ToLower() == "line" && anaData[0] == "Line")
                { 
                    DrawLine(ref subPointData, picPen);
                }

                if (commData[0].ToLower() == "dashline" && anaData[0] == "DashLine")
                {
                    DrawLine(ref subPointData, picPen, true);
                }
            }
            #endregion

            #region pointData.Count == 1
            //例: j = line(A, B, 11)[110]
            else if (pointData.Count == 1)
            {
                //subIdent[0] - Ident対象名
                //subIdent[1] - Ident対象名
                //subIdent[2] - Ident対象の「ObjName」配列中の位置
                //subIdent[3] - Ident対象の「ObjName」配列中の位置
                int looptime = 0;
                string subComm, subAna;
                string[] subCommData, subAnaData;
                ArrayList subIdent = new ArrayList();
                ArrayList subPointData = new ArrayList();

                for (int i = 0; i < anaData.Length; i++)
                {
                    if (anaData[i] == "Ident")
                    {
                        subIdent.Add(commData[i]);
                    }
                }

                looptime = subIdent.Count;

                if (ObjectFinder(subIdent[0].ToString()) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent[0] + "Error");
                    return;
                }

                if (ObjectFinder(subIdent[1].ToString()) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent[1] + "Error");
                    return;
                }

                subIdent.Add(ObjectFinder(subIdent[0].ToString()));
                subIdent.Add(ObjectFinder(subIdent[1].ToString()));

                for (int loopi = 0; loopi < looptime; loopi++)
                {
                    subComm = ObjCommand[Convert.ToInt32(subIdent[loopi + 2])].ToString();
                    subAna = ObjAnalysis[Convert.ToInt32(subIdent[loopi + 2])].ToString();
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
                }

                subPointData.Add(pointData[pointData.Count - 1]);
                LogOutput("LIne new mode debugging");
                //distance = 0.0;
                distance = Math.Sqrt((Convert.ToDouble(subPointData[0]) - Convert.ToDouble(subPointData[3])) * (Convert.ToDouble(subPointData[0]) - Convert.ToDouble(subPointData[3])) + (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[4])) * (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[4])));
                //y = Ax + B
                slopeA = Math.Round((Convert.ToDouble(subPointData[3]) - Convert.ToDouble(subPointData[1])) / (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[0])), 5);
                //データの有効性判断
                if (IsInfinityOrNaN(ref slopeA))
                {
                    //垂直処理
                    //InterceptB = Convert.ToDouble(subPointData[3]);
                    //alpha = Math.Atan(slopeA);

                    unKnownX = Convert.ToDouble(subPointData[2]);
                    unKnownY = Convert.ToDouble(subPointData[3]) + Convert.ToDouble(subPointData[4]);
                }

                else if (!IsInfinityOrNaN(ref slopeA) && slopeA == 0)
                {
                    //平行処理
                    unKnownX = Convert.ToDouble(subPointData[2]) + Convert.ToDouble(subPointData[4]);
                    unKnownY = Convert.ToDouble(subPointData[3]);
                }

                else
                {
                    //通常処理
                    InterceptB = Convert.ToDouble(subPointData[3]) - slopeA * Convert.ToDouble(subPointData[2]);
                    alpha = Math.Atan(slopeA);

                    if (slopeA > 0)
                    { 
                        unKnownX = Math.Round(Convert.ToDouble(subPointData[2]) + Convert.ToDouble(subPointData[4]) * Math.Cos(alpha), 2);
                        unKnownY = Math.Round(Convert.ToDouble(subPointData[3]) + Convert.ToDouble(subPointData[4]) * Math.Sin(alpha), 2);
                    }

                    else
                    {
                        unKnownX = Math.Round(Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[4]) * Math.Cos(alpha), 2);
                        unKnownY = Math.Round(Convert.ToDouble(subPointData[3]) - Convert.ToDouble(subPointData[4]) * Math.Sin(alpha), 2);
                    }
                }

                ArrayList temp_pointData = new ArrayList();
                //Pen picPen = new Pen(Color.Black, 0.1F);
                temp_pointData.Add(subPointData[0]); temp_pointData.Add(subPointData[1]);
                temp_pointData.Add(unKnownX); temp_pointData.Add(unKnownY);

                if (commData[0].ToLower() == "line" && anaData[0] == "Line")
                {
                    DrawLine(ref temp_pointData, picPen);
                }

                if (commData[0].ToLower() == "dashline" && anaData[0] == "DashLine")
                {
                    DrawLine(ref temp_pointData, picPen, true);
                }
            }
            #endregion

            #region pointData.Count == 2
            //例: j = line(A, 50, 25)[100]    j = line(50, 25, A)[001]
            else if (pointData.Count == 2)
            {
                int subIndex;
                string subComm, subAna;
                string[] subCommData, subAnaData;
                string checker = "", subIdent = "";
                ArrayList subPointData = new ArrayList();

                for (int i = 0; i < anaData.Length; i++)
                {
                    if (anaData[i] == "IntNum" || anaData[i] == "DblNum")
                    {
                        checker += 0;
                        subPointData.Add(commData[i]);
                    }

                    if (anaData[i] == "Ident")
                    {
                        checker += 1;
                        subIdent = commData[i];
                    }
                }

                //LogOutput("checker data is : " + checker);

                if (ObjectFinder(subIdent) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent + "Error");
                    return;
                }

                subIndex = ObjectFinder(subIdent);

                if (checker == "100")
                {
                    subComm = ObjCommand[subIndex].ToString();
                    subAna = ObjAnalysis[subIndex].ToString();
                    AssignRemover(ref subComm);
                    subCommData = Regex.Split(subComm, @"\|", RegexOptions.IgnoreCase);
                    subAnaData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                    for (int i = subAnaData.Length - 1; i > 0; i--)
                    {
                        if (subAnaData[i] == "IntNum" || subAnaData[i] == "DblNum")
                        {
                            subPointData.Insert(0, subCommData[i]);
                        }
                    }
                }

                else if (checker == "001")
                {
                    subComm = ObjCommand[subIndex].ToString();
                    subAna = ObjAnalysis[subIndex].ToString();
                    AssignRemover(ref subComm);
                    subCommData = Regex.Split(subComm, @"\|", RegexOptions.IgnoreCase);
                    subAnaData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                    for (int i = 0; i < subAnaData.Length; i++)
                    {
                        if (subAnaData[i] == "IntNum" || subAnaData[i] == "DblNum")
                        {
                            subPointData.Insert(0, subCommData[i]);
                        }
                    }
                }

                else
                {
                    LogOutput("サポートされていない形です。");
                    LogOutput("checker : " + checker);
                    LogOutput("入力内容 : \r\n" + "ノーマルデータ: " + ObjComm + "\r\n解析データ: " + ObjAna);
                    return;
                }

                if (commData[0].ToLower() == "line" && anaData[0] == "Line")
                {
                    DrawLine(ref subPointData, picPen);
                }

                if (commData[0].ToLower() == "dashline" && anaData[0] == "DashLine")
                {
                    DrawLine(ref subPointData, picPen, true);
                }

            }
            #endregion

            #region pointData.Count == 3
            //例: j = line(A, 50, 25, 11)[1000]    j = line(50, 25, A, 11)[0010]
            else if (pointData.Count == 3)
            {
                int subIndex;
                string subComm, subAna;
                string[] subCommData, subAnaData;
                string checker = "", subIdent = "";
                ArrayList subPointData = new ArrayList();

                for (int i = 0; i < anaData.Length; i++)
                {
                    if (anaData[i] == "IntNum" || anaData[i] == "DblNum")
                    {
                        checker += 0;
                        subPointData.Add(commData[i]);
                    }

                    if (anaData[i] == "Ident")
                    {
                        checker += 1;
                        subIdent = commData[i];
                    }
                }

                //LogOutput("checker data is : " + checker);

                if (ObjectFinder(subIdent) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent + "Error");
                    return;
                }

                subIndex = ObjectFinder(subIdent);

                if (checker == "1000")
                {
                    subComm = ObjCommand[subIndex].ToString();
                    subAna = ObjAnalysis[subIndex].ToString();
                    AssignRemover(ref subComm);
                    subCommData = Regex.Split(subComm, @"\|", RegexOptions.IgnoreCase);
                    subAnaData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                    for (int i = subAnaData.Length - 1; i > 0; i--)
                    {
                        if (subAnaData[i] == "IntNum" || subAnaData[i] == "DblNum")
                        {
                            subPointData.Insert(0, subCommData[i]);
                        }
                    }
                }

                else if (checker == "0010")
                {
                    subComm = ObjCommand[subIndex].ToString();
                    subAna = ObjAnalysis[subIndex].ToString();
                    AssignRemover(ref subComm);
                    subCommData = Regex.Split(subComm, @"\|", RegexOptions.IgnoreCase);
                    subAnaData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                    for (int i = subAnaData.Length - 1; i > 0; i--)
                    {
                        if (subAnaData[i] == "IntNum" || subAnaData[i] == "DblNum")
                        {
                            subPointData.Insert(2, subCommData[i]);
                        }
                    }
                }

                else
                {
                    LogOutput("サポートされていない形です。");
                    LogOutput("checker : " + checker);
                    LogOutput("入力内容 : \r\n" + "ノーマルデータ: " + ObjComm + "\r\n解析データ: " + ObjAna);
                    return;
                }

                LogOutput("LIne new mode debugging");
                //distance = 0.0;
                distance = Math.Sqrt((Convert.ToDouble(subPointData[0]) - Convert.ToDouble(subPointData[3])) * (Convert.ToDouble(subPointData[0]) - Convert.ToDouble(subPointData[3])) + (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[4])) * (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[4])));
                //y = Ax + B
                slopeA = Math.Round((Convert.ToDouble(subPointData[3]) - Convert.ToDouble(subPointData[1])) / (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[0])), 5);
                //データの有効性判断
                if (IsInfinityOrNaN(ref slopeA))
                {
                    //垂直処理
                    //InterceptB = Convert.ToDouble(subPointData[3]);
                    //alpha = Math.Atan(slopeA);

                    unKnownX = Convert.ToDouble(subPointData[2]);
                    unKnownY = Convert.ToDouble(subPointData[3]) + Convert.ToDouble(subPointData[4]);
                }

                else if (!IsInfinityOrNaN(ref slopeA) && slopeA == 0)
                {
                    //平行処理
                    unKnownX = Convert.ToDouble(subPointData[2]) + Convert.ToDouble(subPointData[4]);
                    unKnownY = Convert.ToDouble(subPointData[3]);
                }

                else
                {
                    //通常処理
                    InterceptB = Convert.ToDouble(subPointData[3]) - slopeA * Convert.ToDouble(subPointData[2]);
                    alpha = Math.Atan(slopeA);

                    unKnownX = Math.Round(Convert.ToDouble(subPointData[2]) + Convert.ToDouble(subPointData[4]) * Math.Cos(alpha), 2);
                    unKnownY = Math.Round(Convert.ToDouble(subPointData[3]) + Convert.ToDouble(subPointData[4]) * Math.Sin(alpha), 2);
                }

                ArrayList temp_pointData = new ArrayList();
                //Pen picPen = new Pen(Color.Black, 0.1F);
                temp_pointData.Add(subPointData[0]); temp_pointData.Add(subPointData[1]);
                temp_pointData.Add(unKnownX); temp_pointData.Add(unKnownY);

                if (commData[0].ToLower() == "line" && anaData[0] == "Line")
                {
                    DrawLine(ref temp_pointData, picPen);
                }

                if (commData[0].ToLower() == "dashline" && anaData[0] == "DashLine")
                {
                    DrawLine(ref temp_pointData, picPen, true);
                }
            }
            #endregion

            #region pointData.Count == 4
            //例: j = line(0, 0, 50, 25)[0000]
            else if (commData[0].ToLower() == "line" && anaData[0] == "Line" && pointData.Count == 4)
            {
                DrawLine(ref pointData, picPen);
            }

            else if (commData[0].ToLower() == "dashline" && anaData[0] == "DashLine" && pointData.Count == 4)
            {
                DrawLine(ref pointData, picPen, true);
            }
            #endregion

            #region pointData.Count == 5
            //例: j = line(0, 0, 50, 25, 11)[00000]
            else if (commData[0].ToLower() == "line" && anaData[0] == "Line" && pointData.Count == 5)
            {
                LogOutput("LIne new mode debugging");
                //distance = 0.0;
                distance = Math.Sqrt((Convert.ToDouble(pointData[0]) - Convert.ToDouble(pointData[3])) * (Convert.ToDouble(pointData[0]) - Convert.ToDouble(pointData[3])) + (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[4])) * (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[4])));
                //y = Ax + B
                slopeA = Math.Round((Convert.ToDouble(pointData[3]) - Convert.ToDouble(pointData[1])) / (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[0])), 5);
                //データの有効性判断
                if (IsInfinityOrNaN(ref slopeA))
                {
                    //垂直処理
                    //InterceptB = Convert.ToDouble(pointData[3]);
                    //alpha = Math.Atan(slopeA);

                    unKnownX = Convert.ToDouble(pointData[2]);
                    unKnownY = Convert.ToDouble(pointData[3]) + Convert.ToDouble(pointData[4]);
                }

                else if (!IsInfinityOrNaN(ref slopeA) && slopeA == 0)
                {
                    //平行処理
                    unKnownX = Convert.ToDouble(pointData[2]) + Convert.ToDouble(pointData[4]);
                    unKnownY = Convert.ToDouble(pointData[3]);
                }

                else
                {
                    //通常処理
                    InterceptB = Convert.ToDouble(pointData[3]) - slopeA * Convert.ToDouble(pointData[2]);
                    alpha = Math.Atan(slopeA);

                    unKnownX = Math.Round(Convert.ToDouble(pointData[2]) + Convert.ToDouble(pointData[4]) * Math.Cos(alpha), 2);
                    unKnownY = Math.Round(Convert.ToDouble(pointData[3]) + Convert.ToDouble(pointData[4]) * Math.Sin(alpha), 2);
                }

                ArrayList temp_pointData = new ArrayList();
                //Pen picPen = new Pen(Color.Black, 0.1F);
                temp_pointData.Add(pointData[0]); temp_pointData.Add(pointData[1]);
                temp_pointData.Add(unKnownX); temp_pointData.Add(unKnownY);

                DrawLine(ref temp_pointData, picPen);
            }

            else if (commData[0].ToLower() == "dashline" && anaData[0] == "DashLine" && pointData.Count == 5)
            {
                LogOutput("LIne new mode debugging");
                //distance = 0.0;
                distance = Math.Sqrt((Convert.ToDouble(pointData[0]) - Convert.ToDouble(pointData[3])) * (Convert.ToDouble(pointData[0]) - Convert.ToDouble(pointData[3])) + (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[4])) * (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[4])));
                //y = Ax + B
                slopeA = Math.Round((Convert.ToDouble(pointData[3]) - Convert.ToDouble(pointData[1])) / (Convert.ToDouble(pointData[2]) - Convert.ToDouble(pointData[0])), 5);
                //データの有効性判断
                if (IsInfinityOrNaN(ref slopeA))
                {
                    //垂直処理
                    //InterceptB = Convert.ToDouble(pointData[3]);
                    //alpha = Math.Atan(slopeA);

                    unKnownX = Convert.ToDouble(pointData[2]);
                    unKnownY = Convert.ToDouble(pointData[3]) + Convert.ToDouble(pointData[4]);
                }

                else if (!IsInfinityOrNaN(ref slopeA) && slopeA == 0)
                {
                    //平行処理
                    unKnownX = Convert.ToDouble(pointData[2]) + Convert.ToDouble(pointData[4]);
                    unKnownY = Convert.ToDouble(pointData[3]);
                }

                else
                {
                    //通常処理
                    InterceptB = Convert.ToDouble(pointData[3]) - slopeA * Convert.ToDouble(pointData[2]);
                    alpha = Math.Atan(slopeA);

                    unKnownX = Math.Round(Convert.ToDouble(pointData[2]) + Convert.ToDouble(pointData[4]) * Math.Cos(alpha), 2);
                    unKnownY = Math.Round(Convert.ToDouble(pointData[3]) + Convert.ToDouble(pointData[4]) * Math.Sin(alpha), 2);
                }

                ArrayList temp_pointData = new ArrayList();
                //Pen picPen = new Pen(Color.Black, 0.1F);
                temp_pointData.Add(pointData[0]); temp_pointData.Add(pointData[1]);
                temp_pointData.Add(unKnownX); temp_pointData.Add(unKnownY);

                DrawLine(ref temp_pointData, picPen, true);
            }
            #endregion

            else
            {
                tobeRead.SpeakAsync("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!"); 
                codeOutput("@Graphic_LinesClass Draw_LineMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
            }
        }

        /// <summary>
        /// 直線を描く関数の入口，命令文と解析データを入力すれば，直線を描く
        /// この関数は，今まで
        /// </summary>
        /// <param name="ObjComm"></param>
        /// <param name="ObjAna"></param>
        public void Draw_ExLineMode(string ObjComm, string ObjAna)
        {
            LogOutput("Draw_ExLineMode Debugging Start.");
            //LogOutput("Parameter 1  -> " + ObjComm);
            //LogOutput("Parameter 2  -> " + ObjAna + "\r\n");
            //Parameter 1-> Obj|=|exline|(|A|,|40|,|90|)
            //distance -> 40; alpha -> 90
            //Parameter 2-> ExLine|Lparen|Ident|Comma|IntNum|Comma|IntNum|Rparen

            //Define
            double distance = 0.0;
            #region 伸ばすモード 用パラメーター
            double unKnownX = 0, unKnownY = 0, alpha = 0;
            //double slopeA = 0, InterceptB = 0;
            #endregion
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

            //例: obj = exline(Q, 11, 45)[100]
            #region pointData.Count -> 2
            if (commData[0].ToLower() == "exline" && anaData[0] == "ExLine" && pointData.Count == 2)
            {
                int subIndex;
                string subComm, subAna;
                string[] subCommData, subAnaData;
                string checker = "", subIdent = "";
                ArrayList subPointData = new ArrayList();

                for (int i = 0; i < anaData.Length; i++)
                {
                    if (anaData[i] == "IntNum" || anaData[i] == "DblNum")
                    {
                        checker += 0;
                        subPointData.Add(commData[i]);
                    }

                    if (anaData[i] == "Ident")
                    {
                        checker += 1;
                        subIdent = commData[i];
                    }
                }

                if (ObjectFinder(subIdent) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent + "Error");
                    return;
                }

                subIndex = ObjectFinder(subIdent);

                if (checker == "100")
                {
                    subComm = ObjCommand[subIndex].ToString();
                    subAna = ObjAnalysis[subIndex].ToString();
                    AssignRemover(ref subComm);
                    subCommData = Regex.Split(subComm, @"\|", RegexOptions.IgnoreCase);
                    subAnaData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                    for (int i = subAnaData.Length - 1; i > 0; i--)
                    {
                        if (subAnaData[i] == "IntNum" || subAnaData[i] == "DblNum")
                        {
                            subPointData.Insert(0, subCommData[i]);
                        }
                    }
                }

                else
                {
                    LogOutput("サポートされていない形です。");
                    LogOutput("checker : " + checker);
                    LogOutput("入力内容 : \r\n" + "ノーマルデータ: " + ObjComm + "\r\n解析データ: " + ObjAna);
                    return;
                }

                //終点を計算する
                alpha = Convert.ToDouble(subPointData[3].ToString());
                subPointData.RemoveAt(3);
                distance = Convert.ToDouble(subPointData[2].ToString());
                subPointData.RemoveAt(2);

                if (alpha > 0 && alpha < 90)
                {
                    unKnownX = Math.Round(Math.Cos(alpha) * distance + Convert.ToDouble(subPointData[0].ToString()), 2);
                    unKnownY = Math.Round(Math.Sin(alpha) * distance + Convert.ToDouble(subPointData[1].ToString()), 2);

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawLine(ref subPointData, picPen);
                }

                else if (alpha == 90 || alpha == -270)
                {
                    unKnownX = Convert.ToDouble(subPointData[0].ToString());
                    unKnownY = Convert.ToDouble(subPointData[1].ToString()) + distance;

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawLine(ref subPointData, picPen);
                }

                else if (alpha == 180 || alpha == -180)
                {
                    unKnownX = Convert.ToDouble(subPointData[0].ToString()) - distance;
                    unKnownY = Convert.ToDouble(subPointData[1].ToString());

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawLine(ref subPointData, picPen);
                }

                else if (alpha == -90 || alpha == 270)
                {
                    unKnownX = Convert.ToDouble(subPointData[0].ToString());
                    unKnownY = Convert.ToDouble(subPointData[1].ToString()) - distance;

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawLine(ref subPointData, picPen);
                }

                else if (alpha == 0 || alpha == 360 || alpha == -360)
                {
                    unKnownX = Convert.ToDouble(subPointData[0].ToString()) + distance;
                    unKnownY = Convert.ToDouble(subPointData[1].ToString());

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawLine(ref subPointData, picPen);
                }

                else
                {
                    LogOutput("Error in Function Draw_ExLineMode" + "Alpha is over 0 between 90, and 0/90/180/270/360/-90/-180/-270/-360. Please reinput the data!");
                    tobeRead.SpeakAsync("Error in Function Draw_ExLineMode" + "Alpha is over 0 between 90, and 0/90/180/270/360/-90/-180/-270/-360. Please reinput the data!");
                    return;
                }
                //ここまで、角度処理終わり
            }
            #endregion

            else
            {
                LogOutput("入力したデータが識別失敗、この関数の形は「命令名」exline プラス括弧、括弧の中にパラメータは座標点、長さと角度の形です。");
                tobeRead.SpeakAsync("入力したデータが識別失敗、この関数の形は「命令名」exline プラス括弧、括弧の中にパラメータは座標点、長さと角度の形です。");
                return;
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
            pointAx = Convert.ToSingle(pointData[0]) + offset + Point_Offset.X;
            pointAy = Convert.ToSingle(pointData[1]) + offset + Point_Offset.Y;
            pointBx = Convert.ToSingle(pointData[2]) + offset + Point_Offset.X;
            pointBy = Convert.ToSingle(pointData[3]) + offset + Point_Offset.Y;
            
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
            #region 伸ばすモード 用パラメーター
            double distance = 0.0;
            double unKnownX = 0, unKnownY = 0;
            double slopeA = 0, InterceptB = 0, alpha = 0;
            #endregion
            string[] anaData, commData;
            string backObjComm = ObjComm;
            ArrayList pointData = new ArrayList();
            Pen picPen = new Pen(Color.Black, 1F);

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

            #region pointData.Count == 0
            //例: j = line(A, B)[11]
            if (pointData.Count == 0)
            {
                //subIdent[0] - Ident対象名
                //subIdent[1] - Ident対象名
                //subIdent[2] - Ident対象の「ObjName」配列中の位置
                //subIdent[3] - Ident対象の「ObjName」配列中の位置
                int looptime = 0;
                string subComm, subAna;
                string[] subCommData, subAnaData;
                ArrayList subIdent = new ArrayList();
                ArrayList subPointData = new ArrayList();

                for (int i = 0; i < anaData.Length; i++)
                {
                    if (anaData[i] == "Ident")
                    {
                        subIdent.Add(commData[i]);
                    }
                }

                looptime = subIdent.Count;

                if (ObjectFinder(subIdent[0].ToString()) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent[0] + "Error");
                    return;
                }

                if (ObjectFinder(subIdent[1].ToString()) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent[1] + "Error");
                    return;
                }

                subIdent.Add(ObjectFinder(subIdent[0].ToString()));
                subIdent.Add(ObjectFinder(subIdent[1].ToString()));

                for (int loopi = 0; loopi < looptime; loopi++)
                {
                    subComm = ObjCommand[Convert.ToInt32(subIdent[loopi + 2])].ToString();
                    subAna = ObjAnalysis[Convert.ToInt32(subIdent[loopi + 2])].ToString();
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
                }

                //Check The Data Length
                if (subPointData.Count != 4)
                {
                    return;
                }

                if (commData[0].ToLower() == "arrow" && anaData[0] == "Arrow")
                {
                    DrawArrow(ref subPointData, picPen);
                }

                if (commData[0].ToLower() == "dasharrow" && anaData[0] == "Dasharrow")
                {
                    DrawArrow(ref subPointData, picPen, true);
                }
            }
            #endregion

            #region pointData.Count == 1
            //例: j = line(A, B, 11)[110]
            else if (pointData.Count == 1)
            {
                //subIdent[0] - Ident対象名
                //subIdent[1] - Ident対象名
                //subIdent[2] - Ident対象の「ObjName」配列中の位置
                //subIdent[3] - Ident対象の「ObjName」配列中の位置
                int looptime = 0;
                string subComm, subAna;
                string[] subCommData, subAnaData;
                ArrayList subIdent = new ArrayList();
                ArrayList subPointData = new ArrayList();

                for (int i = 0; i < anaData.Length; i++)
                {
                    if (anaData[i] == "Ident")
                    {
                        subIdent.Add(commData[i]);
                    }
                }

                looptime = subIdent.Count;

                if (ObjectFinder(subIdent[0].ToString()) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent[0] + "Error");
                    return;
                }

                if (ObjectFinder(subIdent[1].ToString()) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent[1] + "Error");
                    return;
                }

                subIdent.Add(ObjectFinder(subIdent[0].ToString()));
                subIdent.Add(ObjectFinder(subIdent[1].ToString()));

                for (int loopi = 0; loopi < looptime; loopi++)
                {
                    subComm = ObjCommand[Convert.ToInt32(subIdent[loopi + 2])].ToString();
                    subAna = ObjAnalysis[Convert.ToInt32(subIdent[loopi + 2])].ToString();
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
                }

                subPointData.Add(pointData[pointData.Count - 1]);
                LogOutput("LIne new mode debugging");
                //distance = 0.0;
                distance = Math.Sqrt((Convert.ToDouble(subPointData[0]) - Convert.ToDouble(subPointData[3])) * (Convert.ToDouble(subPointData[0]) - Convert.ToDouble(subPointData[3])) + (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[4])) * (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[4])));
                //y = Ax + B
                slopeA = Math.Round((Convert.ToDouble(subPointData[3]) - Convert.ToDouble(subPointData[1])) / (Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[0])), 5);
                //データの有効性判断
                if (IsInfinityOrNaN(ref slopeA))
                {
                    //垂直処理
                    //InterceptB = Convert.ToDouble(subPointData[3]);
                    //alpha = Math.Atan(slopeA);

                    unKnownX = Convert.ToDouble(subPointData[2]);
                    unKnownY = Convert.ToDouble(subPointData[3]) + Convert.ToDouble(subPointData[4]);
                }

                else if (!IsInfinityOrNaN(ref slopeA) && slopeA == 0)
                {
                    //平行処理
                    unKnownX = Convert.ToDouble(subPointData[2]) + Convert.ToDouble(subPointData[4]);
                    unKnownY = Convert.ToDouble(subPointData[3]);
                }

                else
                {
                    //通常処理
                    InterceptB = Convert.ToDouble(subPointData[3]) - slopeA * Convert.ToDouble(subPointData[2]);
                    alpha = Math.Atan(slopeA);

                    if (slopeA > 0)
                    {
                        unKnownX = Math.Round(Convert.ToDouble(subPointData[2]) + Convert.ToDouble(subPointData[4]) * Math.Cos(alpha), 2);
                        unKnownY = Math.Round(Convert.ToDouble(subPointData[3]) + Convert.ToDouble(subPointData[4]) * Math.Sin(alpha), 2);
                    }

                    else
                    {
                        unKnownX = Math.Round(Convert.ToDouble(subPointData[2]) - Convert.ToDouble(subPointData[4]) * Math.Cos(alpha), 2);
                        unKnownY = Math.Round(Convert.ToDouble(subPointData[3]) - Convert.ToDouble(subPointData[4]) * Math.Sin(alpha), 2);
                    }
                }

                ArrayList temp_pointData = new ArrayList();
                //Pen picPen = new Pen(Color.Black, 0.1F);
                temp_pointData.Add(subPointData[0]); temp_pointData.Add(subPointData[1]);
                temp_pointData.Add(unKnownX); temp_pointData.Add(unKnownY);

                if (commData[0].ToLower() == "arrow" && anaData[0] == "Arrow")
                {
                    DrawArrow(ref temp_pointData, picPen);
                }

                if (commData[0].ToLower() == "dasharrow" && anaData[0] == "DashArrow")
                {
                    DrawArrow(ref temp_pointData, picPen, true);
                }
            }
            #endregion

            else if (commData[0].ToLower() == "arrow" && anaData[0] == "Arrow" && pointData.Count == 4)
            {
                DrawArrow(ref pointData, picPen);
            }

            else if (commData[0].ToLower() == "dasharrow" && anaData[0] == "DashArrow" && pointData.Count == 4)
            {
                DrawArrow(ref pointData, picPen, true);
            }

            else
            {
                tobeRead.SpeakAsync("@Graphic_LinesClass Draw_ArrowMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_LinesClass Draw_ArrowMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
            }
        }

        public void Draw_ExArrowMode(string ObjComm, string ObjAna)
        {
            LogOutput("Draw_ExArrowMode Debugging Start.");
            //LogOutput("Parameter 1  -> " + ObjComm);
            //LogOutput("Parameter 2  -> " + ObjAna + "\r\n");
            //Parameter 1-> Obj|=|exarrow|(|A|,|40|,|90|)
            //distance -> 40; alpha -> 90
            //Parameter 2-> ExArrow|Lparen|Ident|Comma|IntNum|Comma|IntNum|Rparen

            //Define
            double distance = 0.0;
            #region 伸ばすモード 用パラメーター
            double unKnownX = 0, unKnownY = 0, alpha = 0;
            //double slopeA = 0, InterceptB = 0;
            #endregion
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

            //例: obj = exarrow(Q, 11, 45)[100]
            if (commData[0].ToLower() == "exarrow" && anaData[0] == "ExArrow" && pointData.Count == 2)
            {
                int subIndex;
                string subComm, subAna;
                string[] subCommData, subAnaData;
                string checker = "", subIdent = "";
                ArrayList subPointData = new ArrayList();

                for (int i = 0; i < anaData.Length; i++)
                {
                    if (anaData[i] == "IntNum" || anaData[i] == "DblNum")
                    {
                        checker += 0;
                        subPointData.Add(commData[i]);
                    }

                    if (anaData[i] == "Ident")
                    {
                        checker += 1;
                        subIdent = commData[i];
                    }
                }

                if (ObjectFinder(subIdent) == -1)
                {
                    //LogOutput("Error");
                    tobeRead.SpeakAsync(subIdent + "Error");
                    return;
                }

                subIndex = ObjectFinder(subIdent);

                if (checker == "100")
                {
                    subComm = ObjCommand[subIndex].ToString();
                    subAna = ObjAnalysis[subIndex].ToString();
                    AssignRemover(ref subComm);
                    subCommData = Regex.Split(subComm, @"\|", RegexOptions.IgnoreCase);
                    subAnaData = Regex.Split(subAna, @"\|", RegexOptions.IgnoreCase);

                    for (int i = subAnaData.Length - 1; i > 0; i--)
                    {
                        if (subAnaData[i] == "IntNum" || subAnaData[i] == "DblNum")
                        {
                            subPointData.Insert(0, subCommData[i]);
                        }
                    }
                }

                else
                {
                    LogOutput("サポートされていない形です。");
                    LogOutput("checker : " + checker);
                    LogOutput("入力内容 : \r\n" + "ノーマルデータ: " + ObjComm + "\r\n解析データ: " + ObjAna);
                    return;
                }

                //終点を計算する
                alpha = Convert.ToDouble(subPointData[3].ToString());
                subPointData.RemoveAt(3);
                distance = Convert.ToDouble(subPointData[2].ToString());
                subPointData.RemoveAt(2);

                if (alpha > 0 && alpha < 90)
                {
                    unKnownX = Math.Round(Math.Cos(alpha) * distance + Convert.ToDouble(subPointData[0].ToString()), 2);
                    unKnownY = Math.Round(Math.Sin(alpha) * distance + Convert.ToDouble(subPointData[1].ToString()), 2);

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawArrow(ref subPointData, picPen);
                }

                else if (alpha == 90 || alpha == -270)
                {
                    unKnownX = Convert.ToDouble(subPointData[0].ToString());
                    unKnownY = Convert.ToDouble(subPointData[1].ToString()) + distance;

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawArrow(ref subPointData, picPen);
                }

                else if (alpha == 180 || alpha == -180)
                {
                    unKnownX = Convert.ToDouble(subPointData[0].ToString()) - distance;
                    unKnownY = Convert.ToDouble(subPointData[1].ToString());

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawArrow(ref subPointData, picPen);
                }

                else if (alpha == -90 || alpha == 270)
                {
                    unKnownX = Convert.ToDouble(subPointData[0].ToString());
                    unKnownY = Convert.ToDouble(subPointData[1].ToString()) - distance;

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawArrow(ref subPointData, picPen);
                }

                else if (alpha == 0 || alpha == 360 || alpha == -360)
                {
                    unKnownX = Convert.ToDouble(subPointData[0].ToString()) + distance;
                    unKnownY = Convert.ToDouble(subPointData[1].ToString());

                    subPointData.Add(unKnownX);
                    subPointData.Add(unKnownY);
                    LogOutput("unKnownX-" + unKnownX + "unKnownY-" + unKnownY);

                    DrawArrow(ref subPointData, picPen);
                }

                else
                {
                    LogOutput("Error in Function Draw_ExArrowMode" + "Alpha is over 0 between 90, and 0/90/180/270/360/-90/-180/-270/-360. Please reinput the data!");
                    tobeRead.SpeakAsync("Error in Function Draw_ExArrowMode" + "Alpha is over 0 between 90, and 0/90/180/270/360/-90/-180/-270/-360. Please reinput the data!");
                    return;
                }
            }

            else
            {
                LogOutput("入力したデータが識別失敗、この関数の形は「命令名」ExArrow プラス括弧、括弧の中にパラメータは座標点、長さと角度の形です。");
                tobeRead.SpeakAsync("入力したデータが識別失敗、この関数の形は「命令名」exarrow プラス括弧、括弧の中にパラメータは座標点、長さと角度の形です。");
                return;
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
            pointAx = Convert.ToSingle(pointData[0]) + offset + Point_Offset.X;
            pointAy = Convert.ToSingle(pointData[1]) + offset + Point_Offset.Y;
            pointBx = Convert.ToSingle(pointData[2]) + offset + Point_Offset.X;
            pointBy = Convert.ToSingle(pointData[3]) + offset + Point_Offset.Y;

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

        private void Extend()
        { }
    }
}
