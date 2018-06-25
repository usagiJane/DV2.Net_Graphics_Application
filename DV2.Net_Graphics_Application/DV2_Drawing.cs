using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region Personal Addition
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        //DV2
        MyDotView mdv;
        //private const float pub_offSet = 35f;
        private const float pub_offSet = 5f;
        private readonly Pen pub_picPen = new Pen(Color.LightBlue, 2.7F);
        
        public void Dv2ConnectFunction(MainForm fm)
        {
            ConsoleKeyInfo cki;
            // MyDotView インスタンスを取得する
            mdv = MyDotView.getInstance(fm);

            // DotViewに接続する
            mdv.Connect();

            // 表示用の配列を用意する
            int[,] dots = new int[48, 32];
            for (int i = 0; i < 48; i++)
            {
                // 1行目の点を表示
                dots[i, 0] = 1;
                // ２行目の点を点滅
                dots[i, 1] = 2;
            }

            // DotViewにデータを送信する
            mdv.SetDots(dots, 0);

            // アプリケーション終了時には切断処理を呼ぶ
            Console.WriteLine("Press the Escape (Esc) key to quit: \n");
            cki = Console.ReadKey();
            if (cki.Key == ConsoleKey.Escape)
            {
                mdv.Disconnect();
            }
        }

        #region For The Test
        public void line_model_A(PointF pointA, PointF pointB)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(ThreadTest));
            thread.Start("Test");
        }

        static void ThreadTest(object obj)
        {
            Console.WriteLine(obj);
        }

        public void AutoSelectmode()
        {
            
        }
        #endregion

        public void AssignRemover(ref string ObjComm)
        {
            //イコール記号までのデータを削除する
            //ObjComm  "obj1|=|line|(|0|,|0|,|20|,|10|)" -> "line|(|0|,|0|,|20|,|10|)"
            //Define
            bool assignFlag = false;
            int loop_i;

            //Debug
            LogOutput("Processing @AssignRemover");

            for (loop_i = 0; loop_i < Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase).Length; loop_i++)
            {
                if (Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase)[loop_i] == "=")
                {
                    assignFlag = true;
                    break;
                }
            }

            if (assignFlag)
            {
                ObjComm = String.Join("|", Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase).Skip(loop_i + 1).ToArray());
                //ObjComm = ObjComm.Substring(0, ObjComm.Length - 1);
                LogOutput("Assign Remove Success!");
            }
            else
            {
                LogOutput("Can't Find Assign!");
            }
        }

        public int IdentFinder(ref string[] chkData)
        {
            //chkData   "["circle", "(", "c", ",", "5", ")"]"
            //Identデータ番号を探す
            for (int i = 0; i < chkData.Length; i++)
            {
                if (chkData[i] == "Ident")
                {
                    return i;
                }
            }
            return -1;
        }

        public int ObjectFinder(string targetName)
        {
            //Find the targetName in the ArrayList ObjName
            int index = 0;

            foreach (string finder in ObjName)
            {
                if (finder == targetName)
                {
                    return index;
                }
                index += 1;
            }
            return -1;
        }

        public void GetLinePoints(string lineName, ref List<double> linePoints)
        {
            //直線座標点の集合を計算する
            //Define
            int index;
            double slope;
            string targetComm;
            string[] targetCommLis;
            string[] targetAnaLis;
            Point theMaxX = new Point();
            Point theMinX = new Point();
            List<double> pointData = new List<double>();
            List<double> subLinePoints = new List<double>();

            index = ObjectFinder(lineName);

            if (index == -1)
            {
                //Error
                return;
            }

            //座標点を取り出す
            targetComm = ObjCommand[index].ToString();
            AssignRemover(ref targetComm);
            targetCommLis = Regex.Split(targetComm, @"\|", RegexOptions.IgnoreCase);
            targetAnaLis = Regex.Split(ObjAnalysis[index].ToString(), @"\|", RegexOptions.IgnoreCase);

            for (int i = 0; i < targetAnaLis.Length; i++)
            {
                if (targetAnaLis[i] == "IntNum" || targetAnaLis[i] == "DblNum")
                {
                    pointData.Add(Convert.ToDouble(targetCommLis[i]));
                }
            }

            if (Math.Max(pointData[1], pointData[3]) == pointData[1])
            {
                theMaxX.X = Convert.ToInt32(pointData[1]);
                theMaxX.Y = Convert.ToInt32(pointData[2]);
                theMinX.X = Convert.ToInt32(pointData[3]);
                theMinX.Y = Convert.ToInt32(pointData[4]);
            }
            else
            {
                theMaxX.X = Convert.ToInt32(pointData[3]);
                theMaxX.Y = Convert.ToInt32(pointData[4]);
                theMinX.X = Convert.ToInt32(pointData[1]);
                theMinX.Y = Convert.ToInt32(pointData[2]);
            }

            slope = ((double)(theMinX.Y - theMaxX.Y)) / (theMinX.X - theMaxX.X);

            for (int i = theMinX.X + 1; i < theMaxX.X; i++)
            {
                double y = slope * (i - theMinX.X) + theMinX.Y;
                double temp = y - Convert.ToInt32(y);

                if (0.001 > temp && temp > -0.001)
                {
                    subLinePoints.Add(i);
                    subLinePoints.Add(Convert.ToInt32(temp));
                }
            }

            //return;
            if (subLinePoints.Count() % 2 == 0)
            {
                linePoints = subLinePoints;
            }
            else
            {
                //Error
                return;
            }
        }

    }
}