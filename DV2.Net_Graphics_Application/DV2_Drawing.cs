using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region Personal Addition
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections;
using DV = KGS.Tactile.Display;
using System.Windows.Forms;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        //private const float pub_offSet = 35f;
        private const float pub_offSet = 5f;
        private readonly Pen pub_picPen = new Pen(Color.LightBlue, 2.7F);
        //表示用の配列を用意する
        private int[,] forDisDots = new int[48, 32];

        public void Dv2ConnectFunction(MainForm fm)
        {
            //ConsoleKeyInfo cki;
            // MyDotView インスタンスを取得する
            Dv2Instance = MyDotView.getInstance(fm);

            // DotViewに接続する
            Dv2Instance.Connect();

            for (int i = 24; i < 48; i++)
            {
                // 1行目の点を表示
                forDisDots[i, 20] = 1;
                // 2行目の点を点滅
                forDisDots[i, 10] = 2;
                // 3行目の点を表示
                forDisDots[i, 2] = 1;
            }

            // DotViewにデータを送信する
            //Dv2Instance.SetDots(forDisDots, BlinkInterval);

            #region Useless
            // アプリケーション終了時には切断処理を呼ぶ
            /*
            Console.WriteLine("Press the Escape (Esc) key to quit: \n");
            try
            { 
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Escape)
                {
                    Dv2Instance.Disconnect();
                }
            }
            catch(Exception ex)
            {
                Dv2Instance.Disconnect();
                codeOutput(ex.Message);
            }
            */
            #endregion

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

        public void GetLinePoints(string lineName, ref List<int> linePoints)
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
            List<int> subLinePoints = new List<int>();

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

        public void GetCirclePoints(string cirName, ref List<int> cirPoints)
        {
            //円座標点の集合を計算する
            //Define
            int index;
            int radius;
            string targetComm;
            string[] targetCommLis;
            string[] targetAnaLis;
            Point theCenter = new Point();
            List<double> pointData = new List<double>();
            List<int> subCirPoints = new List<int>();

            index = ObjectFinder(cirName);

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

            if (pointData.Count != 3)
            {
                //Error
                return;
            }
            theCenter.X = Convert.ToInt32(pointData[0]);
            theCenter.Y = Convert.ToInt32(pointData[1]);
            radius = Convert.ToInt32(Math.Round(pointData[2]));

            for (int i = 0; i <= radius; i++)
            {
                for (int degree = 0; degree < 360; degree++)
                {
                    
                    subCirPoints.Add(theCenter.X + radius * Convert.ToInt32(Math.Round(Math.Cos(degree * (Math.PI / 180)))));
                    subCirPoints.Add(theCenter.Y + radius * Convert.ToInt32(Math.Round(Math.Sin(degree * (Math.PI / 180)))));
                }
            }

            //return;
            if (subCirPoints.Count() % 2 == 0)
            {
                cirPoints = subCirPoints;
            }
            else
            {
                //Error
                return;
            }
        }

        private void KeyHandleEvent(object sender, DV.KeyEventArgs e)
        {
            tobeRead.SpeakAsyncCancelAll();
            codeOutput("Shift -+-> " + e.Shift);
            codeOutput("Kind -+-> " + e.Kind);
            codeOutput("Value -+-> " + e.Value);
            //DotViewの何かのキーが押された時の処理
            //手前両端上のボタン
            if (e.Shift == 128)
            {
                tobeRead.SpeakAsync("確定");
            }

            //中央キー
            if (e.Shift == 16)
            {

            }

            //ステータスキー
            if (e.Shift == 64)
            {
                tobeRead.SpeakAsync("削除");
            }

            //縮小キー(順番に選択状態にする)
            if (e.Kind == 4 && e.Value == 16)
            {
                tobeRead.SpeakAsync("挿入キャンセル");
            }

            //拡大キー(１つ前の操作した状態に戻る)
            if (e.Kind == 4 && e.Value == 128)
            {
                tobeRead.SpeakAsync("挿入キャンセル");
            }

            //レバー上
            if (e.Kind == 5 && e.Value == 8)
            {
                
            }

            //レバー下
            if (e.Kind == 5 && e.Value == 4)
            {

            }

            //レバー左
            if (e.Kind == 5 && e.Value == 16)
            {

            }

            //レバー右
            if (e.Kind == 5 && e.Value == 2)
            {
                
            }

            //拡大
            if (e.Kind == 4 && e.Value == 2)
            {
                tobeRead.SpeakAsync("拡大");
            }

            //縮小
            if (e.Kind == 4 && e.Value == 1)
            {
                tobeRead.SpeakAsync("縮小");
            }

            //側面矢印中央
            if (e.Shift == 32)
            {
                tobeRead.SpeakAsync("最小角度編集");
                tobeRead.SpeakAsync("最大角度編集");
                tobeRead.SpeakAsync("通常回転");
                tobeRead.SpeakAsync("回転");
            }

            // F5キー：四角形x方向拡大
            if (e.Kind == 4 && e.Value == 4)
            {
                tobeRead.SpeakAsync("拡大横");
            }

            if (e.Kind == 4 && e.Value == 8)
            {
                tobeRead.SpeakAsync("縮小横");
            }

            if (e.Kind == 4 && e.Value == 8)
            {
                tobeRead.SpeakAsync("右回転");
                tobeRead.SpeakAsync("角度減少");
            }

            if (e.Kind == 4 && e.Value == 4)
            {
                tobeRead.SpeakAsync("左回転");
                tobeRead.SpeakAsync("角度増加");
            }

            //タグモード
            if (e.Kind == 4 && e.Value == 32)
            {
                //syn.SpeakAsync("タグモード");
            }

            //アンドゥモード
            if (e.Kind == 4 && e.Value == 64)
            {
                tobeRead.SpeakAsync("アンドゥモード");
            }

        }

        private void MouseMoveEvent(object sender, MouseEventArgs e)
        {
            /*
            label1.Text = Cursor.Position.X.ToString();
            label2.Text = Cursor.Position.Y.ToString();
            X1 = Cursor.Position.X;
            Y1 = Cursor.Position.Y;

            X2 = X1 - 73; //キャリブレーション 校正
            Y2 = Y1 - 180; //キャリブレーション
            X3 = X2 / 15;
            Y3 = Y2 / 12;

            X3 = 48 - X3;
            Y3 = 32 - Y3;

            if (X3 < 0)
                X3 = 0;
            if (X3 > 47)
                X3 = 48;
            if (Y3 < 0)
                Y3 = 0;
            if (Y3 > 31)
                Y3 = 32;

            height = Y3;
            width = X3;
            show_dot();
            */
        }

        private void MakeObjectBraille()
        {
            //この関数はDV2出力データを準備する
            int[,] allDotData = new int[picBox.Size.Height, picBox.Size.Width];
            int[,] focusDotData = new int[48, 32];
            Color pixel;

            for (int hei = 0; hei < picBox.Size.Height; hei++)
            {
                for (int wid = 0; wid < picBox.Size.Width; wid++)
                {
                    //ピクセルデータを0と1に変化する
                    pixel = debug_Image.GetPixel(hei, wid);
                    
                }
            }

        }
    }
}