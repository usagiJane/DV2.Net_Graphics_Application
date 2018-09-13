using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region Personal Addition
using System.Drawing;
using System.Text.RegularExpressions;
using DV = KGS.Tactile.Display;
using System.Windows.Forms;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        //private const float pub_offSet = 35f;
        private const float pub_offSet = 0f;
        private readonly Pen pub_picPen = new Pen(Color.LightBlue, 1F);
        //表示用の配列を用意する
        private int[,] forDisDots = new int[48, 32];
        private int[,] allDotData;
        //movementのlocationは左上
        Point movement;

        public void Dv2ConnectFunction(MainForm fm)
        {
            //ConsoleKeyInfo cki;
            // MyDotView インスタンスを取得する
            Dv2Instance = MyDotView.getInstance(fm);

            DotDataInitialization(ref forDisDots);
            DotDataInitialization(ref allDotData);

            #region Useless
            /*
            for (int i = 24; i < 48; i++)
            {
                // 1行目の点を表示
                forDisDots[i, 20] = 1;
                // 2行目の点を点滅
                forDisDots[i, 10] = 2;
                // 3行目の点を表示
                forDisDots[i, 2] = 1;
            }
            */
            #endregion

            // DotViewにデータを送信する
            Dv2Instance.SetDots(forDisDots, BlinkInterval);

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
                theMaxX.X = Convert.ToInt32(Math.Round(pointData[0]));
                theMaxX.Y = Convert.ToInt32(Math.Round(pointData[1]));
                theMinX.X = Convert.ToInt32(Math.Round(pointData[2]));
                theMinX.Y = Convert.ToInt32(Math.Round(pointData[3]));
            }
            else
            {
                theMaxX.X = Convert.ToInt32(Math.Round(pointData[2]));
                theMaxX.Y = Convert.ToInt32(Math.Round(pointData[3]));
                theMinX.X = Convert.ToInt32(Math.Round(pointData[0]));
                theMinX.Y = Convert.ToInt32(Math.Round(pointData[1]));
            }

            slope = ((double)(theMinX.Y - theMaxX.Y)) / (theMinX.X - theMaxX.X);

            for (int i = theMinX.X + 1; i < theMaxX.X; i++)
            {
                double y = slope * (i - theMinX.X) + theMinX.Y;
                double temp = y - Convert.ToInt32(y);

                if (0.001 > temp && temp > -0.001)
                {
                    subLinePoints.Add(i);
                    subLinePoints.Add(Convert.ToInt32(y));
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

        private void Dv2KeyEventHandle(object sender, DV.KeyEventArgs e)
        {
            bool moved_flg = false;
            tobeRead.SpeakAsyncCancelAll();
            #region Debug
            if (false)
            { 
                codeOutput("************************************************");
                codeOutput("Shift -+-> " + e.Shift);
                codeOutput("Kind -+-> " + e.Kind);
                codeOutput("Value -+-> " + e.Value);
            }
            #endregion

            //DotViewの何かのキーが押された時の処理
            #region フロントキー処理
            // ***************フロントキー処理***************
            if (e.Shift == 128 && e.Kind == 0 && e.Value == 0)
            {
                //親指キー
                tobeRead.SpeakAsync("親指キー");
            }

            if (e.Shift == 64 && e.Kind == 0 && e.Value == 0)
            {
                //ステータスキー
                tobeRead.SpeakAsync("ステータスキー");
            }

            if (e.Shift == 0 && e.Kind == 4 && e.Value == 16)
            {
                //拡大キー
                tobeRead.SpeakAsync("拡大キー");
            }

            if (e.Shift == 0 && e.Kind == 4 && e.Value == 128)
            {
                //縮小キー
                tobeRead.SpeakAsync("縮小キー");
            }

            if (e.Shift == 0 && e.Kind == 5 && e.Value == 8)
            {
                //方向レバー 上
                //tobeRead.SpeakAsync("方向レバー 上");
                movement.Y -= 1;
                moved_flg = true;

                if (movement.Y < 0)
                {
                    movement.Y = 0;
                    moved_flg = false;
                }
            }

            if (e.Shift == 0 && e.Kind == 5 && e.Value == 4)
            {
                //方向レバー 下
                //tobeRead.SpeakAsync("方向レバー 下");
                movement.Y += 1;
                moved_flg = true;

                if (movement.Y + 32 > picBox.Height)
                {
                    movement.Y = picBox.Height - 32;
                    moved_flg = false;
                }
            }

            if (e.Shift == 0 && e.Kind == 5 && e.Value == 16)
            {
                //方向レバー 左
                //tobeRead.SpeakAsync("方向レバー 左");
                movement.X -= 1;
                moved_flg = true;

                if (movement.X < 0)
                {
                    movement.X = 0;
                    moved_flg = false;
                }
            }

            if (e.Shift == 0 && e.Kind == 5 && e.Value == 2)
            {
                //方向レバー 右
                //tobeRead.SpeakAsync("方向レバー 右");
                movement.X += 1;
                moved_flg = true;

                if (movement.X + 48 > picBox.Width)
                {
                    movement.X = picBox.Width - 48;
                    moved_flg = false;
                }
            }

            if (e.Shift == 16 && e.Kind == 0 && e.Value == 0)
            {
                //方向レバー 中心
                tobeRead.SpeakAsync("方向レバー 中心");
            }
            #endregion

            #region サイドキー処理
            // ***************サイドキー処理***************
            if (e.Shift == 0 && e.Kind == 4 && e.Value == 1)
            {
                //上矢印キー
                tobeRead.SpeakAsync("上矢印キー");
            }

            if (e.Shift == 0 && e.Kind == 4 && e.Value == 2)
            {
                //下矢印キー
                tobeRead.SpeakAsync("下矢印キー");
            }

            if (e.Shift == 0 && e.Kind == 4 && e.Value == 8)
            {
                //左矢印キー
                tobeRead.SpeakAsync("左矢印キー");
            }

            if (e.Shift == 0 && e.Kind == 4 && e.Value == 4)
            {
                //右矢印キー
                tobeRead.SpeakAsync("右矢印キー");
            }

            if (e.Shift == 32 && e.Kind == 0 && e.Value == 0)
            {
                //センターキー
                tobeRead.SpeakAsync("センターキー");
            }

            if (e.Shift == 0 && e.Kind == 4 && e.Value == 64)
            {
                //エンドキー
                tobeRead.SpeakAsync("エンドキー");
            }

            if (e.Shift == 0 && e.Kind == 4 && e.Value == 32)
            {
                //ホームキー
                tobeRead.SpeakAsync("ホームキー");
            }
            #endregion

            // *************** 移動処理 ***************
            if (moved_flg)
            {
                DotDataInitialization(ref forDisDots);

                for (int width = 0; width < 48; width++)
                {
                    for (int height = 0; height < 32; height++)
                    {
                        forDisDots[width, height] = allDotData[movement.X + width, movement.Y + height];
                    }
                }
                Dv2Instance.SetDots(forDisDots, BlinkInterval);
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
            //picBox.Size.Height   400
            //picBox.Size.Width    600
            //allDotData   全てのDotデータを保存する場所
            //focusDotData   拡大縮小用Dotデータを保存する場所
            //forDisDots   DV2 Displayを転送するデータ

            //allDotData = new int[picBox.Width, picBox.Height];
            //int[,] focusDotData = new int[48, 32];
            movement = new Point(0, 0);
            Color pixel;

            //データ初期化
            DotDataInitialization(ref allDotData);
            DotDataInitialization(ref forDisDots);
            //DotDataInitialization (ref focusDotData);

            LogOutput("MakeObjectBraille Start!");

            for (int width = 0; width < picBox.Size.Width; width++)
            {
                for (int height = 0; height < picBox.Size.Height; height++)
                {
                    //ピクセルデータを0と1に変化する
                    pixel = debug_Image.GetPixel(width, height);
                    if (pixel.Name != "0")
                    {
                        allDotData[width, height] = 1;

                        if ((width - movement.X) < forDisDots.GetLength(0) && (height - movement.Y) < forDisDots.GetLength(1))
                        {
                            forDisDots[width, height] = 2;
                        }
                    }
                }
            }

            Dv2Instance.SetDots(forDisDots, BlinkInterval);
            //tabControl_code.Enabled = false;
            tabControl_code.SelectTab(0);
            tabControl_Graphics.Enabled = false;
        }

        private void DotDataInitialization(ref int[,] dotDatas)
        {
            int rows = dotDatas.GetLength(0);
            int columns = dotDatas.GetLength(1);

            for (int m = 0; m < rows; m++)
            {
                for (int n = 0; n < columns; n++)
                {
                    dotDatas[m, n] = 0;
                }
            }
        }

        private void KeyMovement(object sender, KeyEventArgs e)
        {
            bool moved_flg = false;
            #region Movement
            if (e.KeyCode == Keys.Up)
            {
                //codeOutput("Up Pressed!");
                movement.Y -= 1;
                moved_flg = true;

                if (movement.Y < 0)
                {
                    movement.Y = 0;
                    moved_flg = false;
                }
            }

            if (e.KeyCode == Keys.Down)
            {
                //codeOutput("Down Pressed!");
                movement.Y += 1;
                moved_flg = true;

                if (movement.Y + 32 > picBox.Height)
                {
                    movement.Y = picBox.Height - 32;
                    moved_flg = false;
                }
            }

            if (e.KeyCode == Keys.Left)
            {
                //codeOutput("Left Pressed!");
                movement.X -= 1;
                moved_flg = true;

                if (movement.X < 0)
                {
                    movement.X = 0;
                    moved_flg = false;
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                //codeOutput("Right Pressed!");
                movement.X += 1;
                moved_flg = true;

                if (movement.X + 48 > picBox.Width)
                {
                    movement.X = picBox.Width - 48;
                    moved_flg = false;
                }
            }

            if (e.KeyCode == Keys.End)
            {
                //codeOutput("End Pressed!");
                tobeRead.SpeakAsync("触るモード終了.");
                tabControl_code.SelectTab(0);
                tabControl_code.Enabled = true;
                tabControl_Graphics.Enabled = true;
            }
            #endregion

            if (moved_flg)
            {
                DotDataInitialization(ref forDisDots);

                for (int width = 0; width < 48; width++)
                {
                    for (int height = 0; height < 32; height++)
                    {
                        forDisDots[width, height] = allDotData[movement.X + width, movement.Y + height];
                    }
                }
                Dv2Instance.SetDots(forDisDots, BlinkInterval);
            }
        }

    }
}