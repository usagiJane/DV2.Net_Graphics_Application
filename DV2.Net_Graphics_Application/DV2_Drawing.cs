using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        //DV2
        MyDotView mdv;
        private const float pub_offSet = 35f;
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

        
    }
}