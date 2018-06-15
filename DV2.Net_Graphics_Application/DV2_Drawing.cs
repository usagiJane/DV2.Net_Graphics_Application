using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#region Appended
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
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
            for (int i = 0; i < chkData.Length; i++)
            {
                if (chkData[i] == "Ident")
                {
                    return i;
                }
            }
            return 0;
        }
    }
}