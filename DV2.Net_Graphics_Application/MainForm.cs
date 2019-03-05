using System;
using System.Data;
using System.Windows.Forms;

#region Personal Addition
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;
using DV = KGS.Tactile.Display;
using System.Speech.Synthesis;
#endregion

#region useless
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Speech.Recognition;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm : Form
    {
        #region ObjStorage
        ArrayList ObjName = new ArrayList();
        ArrayList ObjCommand = new ArrayList();
        ArrayList ObjAnalysis = new ArrayList();
        ArrayList ObjDisplayed = new ArrayList();

        static Graphics graphObj;
        static Bitmap debug_Image;
        private static SpeechSynthesizer tobeRead = new SpeechSynthesizer();
        //FontSizeノーマルは"9"
        private static int FontSize = 24;
        //回転Flag
        bool ROTATIONFLAG = false;
        #endregion

        #region About DV2
        //DV2 Global Parameter
        MyDotView Dv2Instance;
        private int BlinkInterval = 0;
        #endregion

        /// <summary>
        /// Main関数
        /// </summary>
        public MainForm()
        {
            //システム基礎的な関数
            InitializeComponent();
            //紙を用意する
            graphObj = PreparePaper();
            //読み上げる判断
            if (Properties.Settings.Default.NEEDINPUTREADER)
            {
                tobeRead.SpeakAsync("入力した内容を読み上げます。");
            }
            //キーボードイベント関数
            this.textBox_Input.KeyDown += new KeyEventHandler(EnterKeyPress);
            this.tabControl_code.SelectedIndexChanged += new EventHandler(tabControl_code_SelectedIndexChanged);
            this.KeyDown += new KeyEventHandler(KeyMovement);

            #region About the Tablet Event
            //ペンタブレット移動量に関するイベント関数
            MouseMove += new MouseEventHandler(this.TabletMouseMove);
            #endregion

            #region DV2 Key Event
            allDotData = new int[picBox.Width + 48, picBox.Height + 32];
            CheckForIllegalCrossThreadCalls = false;
            Point_Offset.X = picBox.Width / 2;
            Point_Offset.Y = picBox.Height / 2;

            Dv2ConnectFunction(this);
            Dv2Instance.DvCtl.KeyUp += new DV.KeyEventHandler(this.Dv2KeyEventHandle);
            if (DV2.Net_Graphics_Application.Properties.Settings.Default.Dv2_CONNECTED)
            {
                Dv2Instance.Connect();
            }
            #endregion
        }

        /// <summary>
        /// Formを起動する時に，呼び出し関数，一部の初期値が設定可能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formloader(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable("GraphicsDataType");
            dataTable.Columns.Add("Number", typeof(String));
            dataTable.Columns.Add("GraphicsType", typeof(String));
            dataTable.Rows.Add(new String[] { "1", "[;]" });
            dataTable.Rows.Add(new String[] { "2", @"[\r\n]" });
            dataTable.Rows.Add(new String[] { "3", @"[\n]" });

            #region Debug Mode Default Value
            //textBox1.Text = "this is text box 1"; be used to storage input data ->dataStorage
            //Debug
            textBox_CenterCoordinates.ReadOnly = true;
            //textBox3.Text = "this is text box 3";
            textBox_FingerGet.ReadOnly = true;

            //"line(1,1,100,100)||line(30,55,200,255);\r\narc(45,10,170,165,45,45);\r\ncircle(110,150,90,120);\r\narrow(25,25,230,25);\r\ntriangle(90,85,110,125,60,75);";
            //textBox_Input.Text = "obj1=line(0,5,200.0,405.0)";
            //textBox_Input.Text = "obj1=circle(1,2,20.0)";
            //textBox_Input.Text = "obj1=circle(c,20.0)";
            //textBox_Input.Text = "var p : Point";
            //textBox_Input.Text = "get p on obj1";
            //textBox_Input.Text = "c = Point(4, 7)";
            #endregion

            //Draw a border style for the picture box
            picBox.BorderStyle = BorderStyle.FixedSingle;
            label_posX.BorderStyle = BorderStyle.FixedSingle;
            label_posY.BorderStyle = BorderStyle.FixedSingle;
            //Set the table control values
            tabControl_Graphics.Enabled = true;
            tabControl_Graphics.Visible = false;
            tabControl_code.Enabled = true;
            textBox_code.Enabled = false;
            textBox_Input.Focus();
            //this.dataGridView_monitor.Rows.Add();
            InitializationBaseDatas();
            //Debug Image
            DebugImshow();
            //入力フォントサイズ
            textBox_Input.Font = new Font(textBox_Input.Font.FontFamily, FontSize);

            #region Speaker Setting
            //tobeRead.SelectVoice("Microsoft Anna");
            tobeRead.Volume = 100;
            tobeRead.SpeakAsync("キーワードについて、大文字と小文字は区別されていません。");
            #endregion

            //PrimaryScreen Size
            int iActulaWidth = Screen.PrimaryScreen.Bounds.Width;
            int iActulaHeight = Screen.PrimaryScreen.Bounds.Height;
            LogOutput("PrimaryScreen Size is  " + iActulaWidth + " : " + iActulaHeight);

            DriveInfo[] allDirves = DriveInfo.GetDrives();
            Properties.Settings.Default.NEEDINPUTREADER = true;
        }

        /// <summary>
        /// logを出力関数，Log4Netを利用してハードディスクに出力する
        /// </summary>
        /// <param name="log"></param>
        internal void LogOutput(object log)
        {
            //FontSizeノーマルは"9"
            int FontSize = 9;
            textBox_log.Font = new Font(textBox_log.Font.FontFamily, FontSize);
            textBox_log.AppendText(log + "\r\n");
            //textBox_log.ScrollToCaret(); 
            //Write the log file
            DV2SysLogger.Info(log);
        }

        /// <summary>
        /// 入力した内容とユーザー向きに情報を出力関数，Log4Netを利用してハードディスクに出力する
        /// </summary>
        /// <param name="log"></param>
        internal void codeOutput(object log)
        {
            //FontSizeノーマルは"9"
            textBox_code.Font = new Font(textBox_code.Font.FontFamily, FontSize);
            textBox_code.AppendText(log + "\r\n");
            textBox_code.ScrollToCaret();
            //Write the log file
            DV2SysLogger.Info(log);
        }

        /// <summary>
        /// 入力した内容とユーザー向きに情報を出力関数，フォントサイズが指定可能，Log4Netを利用してハードディスクに出力する
        /// </summary>
        /// <param name="log"></param>
        /// <param name="sub_FontSize"></param>
        internal void codeOutput(object log, int sub_FontSize)
        {
            //FontSizeノーマルは"9"
            textBox_code.Font = new Font(textBox_code.Font.FontFamily, sub_FontSize);
            textBox_code.AppendText(log + "\r\n");
            textBox_code.ScrollToCaret();
            //Write the log file
            DV2SysLogger.Info(log);
        }

        /// <summary>
        /// tabControlが変換する際に，ログ画面を自動的にスクロール関数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Debug 
            //MessageBox.Show("tabControl_code_SelectedIndexChanged");
            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.ScrollToCaret();
        }

        /// <summary>
        /// シュミレーターディスプレイを起動する関数
        /// </summary>
        public void DebugImshow()
        {
            tabControl_Graphics.Visible = true;
            //Analog_signal_switch -> False
            if (Properties.Settings.Default.Analog_signal_switch)
            {
                //tabPage_outPutを隠す
                tabPage_outPut.Parent = null;
                if (tabPage_camera.Parent != tabControl_Graphics)
                    tabPage_camera.Parent = tabControl_Graphics;
                //カメラ出力処理
                //CameraStart();
            }
            else
            {
                //tabPage_cameraを隠す
                tabPage_camera.Parent = null;
                if (tabPage_outPut.Parent != tabControl_Graphics)
                    tabPage_outPut.Parent = tabControl_Graphics;
                //Analog出力処理
                picBox.Image = (Image)debug_Image;
            }
        }

        /// <summary>
        /// 入力画面，エンターキーのイベント関数
        /// エンターキーを押しすればこの関数が呼び出す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterKeyPress(object sender, KeyEventArgs e)
        {
            //DV2_Drawing GraphicDraw = new DV2_Drawing();
            #region ToKen Analysis
            if (e.KeyCode == Keys.Enter)
            {
                //Debugのため出力
                //LogOutput(DateTime.Now.ToString("HH:mm") + " textBox_Input 入力データ: " + textBox_Input.Text);
                //input dataをそのまま出力する
                codeOutput(">" + textBox_Input.Text);
                //input dataをbackupする
                dataStorage.Text = textBox_Input.Text;

                if (Properties.Settings.Default.NEEDINPUTREADER)
                {
                    tobeRead.SpeakAsync(dataStorage.Text);
                }

                //input dataを削除する
                textBox_Input.Clear();
                //jump to the sub page
                //tabControl1.SelectTab(1);
                //Backup dataをlogoutする
                LogOutput("*********************************************");
                LogOutput(DateTime.Now.ToString("HH:mm") + " dataStorage データ: " + dataStorage.Text);
                LogOutput("---------------------------------------------" + "\r\n");
                FormulaAnalysis();
            }
            #endregion
        }

        /// <summary>
        /// 描画データを初期化する
        /// </summary>
        /// <returns>Graphics対象データを返す</returns>
        private Graphics PreparePaper()
        {
            //この関数は出力画像データを準備する
            Size picSize = picBox.Size;
            debug_Image = new Bitmap(picSize.Width, picSize.Height);
            //Pen picPen = new Pen(Color.LightBlue, 2.7F);

            Graphics graphObj = Graphics.FromImage(debug_Image);

            LogOutput("picSize.Width: " + picSize.Width);
            LogOutput("picSize.Height: " + picSize.Height);
            //LogOutput("comboBox Pictype SelectedIndex: " + comboBox_codeType.SelectedIndex);
            LogOutput("System String Processing Strated!");

            graphObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphObj.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphObj.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            
            return graphObj;
        }

        /// <summary>
        /// xとy座標軸を描く関数，現段階では利用していない
        /// </summary>
        /// <param name="image"></param>
        /// <param name="graphObj"></param>
        public void drawAxis(ref Bitmap image, ref Graphics graphObj)
        {
            //この関数は座標軸を描く
            //define
            float offset = 35f;
            PointF pX1 = new PointF(0, offset);
            PointF pX2 = new PointF(image.Width - offset, offset);
            PointF pY1 = new PointF(offset, 0);
            PointF pY2 = new PointF(offset, image.Height - offset);
            Pen pArrow = new Pen(Color.Gray, 2);

            //描く
            pArrow.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            graphObj.DrawLine(pArrow, pX1, pX2);
            graphObj.DrawLine(pArrow, pY1, pY2);

            //画像を回転する
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        /// <summary>
        /// xとy座標軸のx軸とy軸の文字を描くため関数，現段階では利用していない
        /// </summary>
        /// <param name="image"></param>
        /// <param name="graphObj"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="partNums"></param>
        public void drawAxisXYpart(ref Bitmap image, ref Graphics graphObj, float minX, float maxX, float minY, float maxY, int partNums)
        {
            //この関数は座標軸の文字を表示する
            //define
            float offset = 35f;
            float lenX = image.Width - 2 * offset;
            float lenY = image.Height - 2 * offset;
            PointF pX1, pX2, pY1, pY2;
            String strAxis;

            for (int i = 1; i < partNums; i++)
            {
                pX1 = new PointF(lenX * i / partNums + offset, image.Height - offset - 3);
                pX2 = new PointF(lenX * i / partNums + offset, image.Height - offset);
                strAxis = ((maxX - minX) * i / partNums + minX).ToString();
                graphObj.DrawLine(new Pen(Color.Black, 2), pX1, pX2);
                graphObj.DrawString(strAxis, new Font("Century", 7f), Brushes.Black, new PointF(lenX * i / partNums + offset - 5, image.Height - offset / 1.1f));
            }
            graphObj.DrawString("X軸", new Font("游明朝 ", 11f), Brushes.Black, new PointF(image.Width - offset / 1.5f, image.Height - offset / 1.5f));

            for (int i = 1; i < partNums; i++)
            {
                pY1 = new PointF(offset, lenY * i / partNums + offset);
                pY2 = new PointF(offset + 3, lenY * i / partNums + offset);
                strAxis = (maxY - (maxY - minY) * i / partNums).ToString();
                graphObj.DrawLine(new Pen(Color.Black, 2), pY1, pY2);
                graphObj.DrawString(strAxis, new Font("Century", 7f), Brushes.Black, new PointF(offset / 2.2f, lenY * i / partNums + offset * 0.8f));
            }
            graphObj.DrawString("Y軸", new Font("游明朝 ", 11f), Brushes.Black, new PointF(offset / 3.5f, offset / 2.7f));
        }
        
        /// <summary>
        /// この関数は入力イベントの動作関数である，入力内容をフィルタリング機能を持っている．
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxKeyPress(object sender, KeyPressEventArgs e)
        {
            //この関数は入力キーをチェックする
            //LogOutput("KeyChar: " + (int)e.KeyChar);
            //"," -> e.KeyChar=44
            //"BackSpace" -> e.KeyChar=8
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 44 && (int)e.KeyChar != 46)
            {
                e.Handled = true;
                //未完待续
            }
            if ((int)e.KeyChar == 46)
            {
                if (textBox_CenterCoordinates.Text.Length <= 0)
                {
                    e.Handled = true;
                }
                else
                {
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(dataStorage.Text, out oldf);
                    b2 = float.TryParse(dataStorage.Text + e.KeyChar.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                            e.Handled = true;
                        else
                            e.Handled = false;
                    }
                }
            }
        }

        /// <summary>
        /// この関数は「機能テスト用」ボタンから呼び出し関数，色々な新機能を試する時に利用する．
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_FFT_Click(object sender, EventArgs e)
        {
            // Button For Functional Test
            MakeObjectBraille();
        }

        /// <summary>
        /// ファイルを読み込み関数
        /// </summary>
        private void button_FS_Click(object sender, EventArgs e)
        {
            //File Selected
            if (openFileDialog_CommandFile.ShowDialog() == DialogResult.OK)
            {
                string content;
                textBox_FS.Text = openFileDialog_CommandFile.FileName.ToString();
                StreamReader sR = new StreamReader(openFileDialog_CommandFile.FileName, Encoding.Default);

                //textBox_log.AppendText(sR.ReadToEnd());
                content = sR.ReadLine();

                while (null != content)
                {
                    //Debug
                    LogOutput(content);
                    if (content.Contains("//"))
                    {
                        content = content.Replace("//", "");
                        tobeRead.SpeakAsync("注釈内容 " + content);
                    }
                    else
                    {
                        dataStorage.Text = content;
                        FormulaAnalysis();
                    }

                    content = sR.ReadLine();
                }

                sR.Close();
            }
        }


    }
    //End of class MainForm
}
