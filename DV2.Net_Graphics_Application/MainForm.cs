﻿using System;
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
        #endregion

        #region About DV2
        //DV2 Global Parameter
        MyDotView Dv2Instance;
        private int BlinkInterval = 0;
        #endregion

        #region About Define ToKen
        public enum TknKind
        {                             /* トークンの種類 */
            Lparen = '(', Rparen = ')', Lbracket = '[', Rbracket = ']', Plus = '+', Minus = '-',
            Multi = '*', Divi = '/', Mod = '%', Not = '!', Ifsub = '?', Assign = '=',
            IntDivi = '\\', Comma = ',', DblQ = '"', Colon = ':',
            Func = 150, Var, If, Elif, Else, For, To, Step, While,
            End, Break, Return, Option, Print, Println, Input, Toint,
            Exit, Equal, NotEq, Less, LessEq, Great, GreatEq, And, Or,
            END_KeyList, 
            Ident, IntNum, DblNum, String, Letter, Doll, Digit,
            Gvar, Lvar, Fcall, EofProg, EofLine, Others, EofTkn, None, END_keylist, END_line,
            /* 図形命令 */
            Line, DashLine, Arrow, DashArrow, Arc, Circle, Triangle, Rectangle, 
            /* 処理関数 */
            Solve, Get, Contact, Show, Clear, Rotation, SetPoint,
            /* 特別関数 */
            With, Point, Set, On, In,
            /* Position 位置*/
            Position, Left, Right, Center, Bottom, Top
        }

        public struct ToKen
        {
            public TknKind kind;                          /* トークンの種類 */
            public string text;
            public double dblVal;                            /* 定数値や変数番号 */

            public ToKen(TknKind tk) { kind = tk; text = ""; dblVal = 0.0; }
            public ToKen(TknKind tk, double d) { kind = tk; text = ""; dblVal = d; }
            public ToKen(TknKind tk, string s) { kind = tk; text = s; dblVal = 0.0; }
            public ToKen(TknKind tk, string s, double d) { kind = tk; text = s; dblVal = d; }
        }

        public struct KeyWord
        {                            /* 字句 */
            public string keyName;
            public TknKind keyKind;         /* 種別 */

            public KeyWord(string keyname, TknKind tknkind) { keyName = keyname; keyKind = tknkind; }
        };

        /* ジャグ配列 */
        KeyWord[] KeyWdTbl = new KeyWord[65];      //要約語と記号の種別対応表
        TknKind[] ctyp = new TknKind[256];         //文字種表定義
        private int loopInfo = 0;
        #endregion

        public MainForm()
        {
            InitializeComponent();
            //紙を用意する
            graphObj = PreparePaper();

            //イベントを宣言する
            this.textBox_Input.KeyDown += new KeyEventHandler(EnterKeyPress);
            this.tabControl_code.SelectedIndexChanged += new EventHandler(tabControl_code_SelectedIndexChanged);
            this.KeyDown += new KeyEventHandler(KeyMovement);

            #region About the Tablet Event
            MouseMove += new MouseEventHandler(this.TabletMouseMove);
            #endregion

            #region DV2 Key Event
            allDotData = new int[picBox.Width + 48, picBox.Height + 32];
            CheckForIllegalCrossThreadCalls = false;
            Point_Offset.X = picBox.Width / 2;
            Point_Offset.Y = picBox.Height / 2;

            Dv2ConnectFunction(this);
            Dv2Instance.DvCtl.KeyUp += new DV.KeyEventHandler(this.Dv2KeyEventHandle);
            if (DV2.Net_Graphics_Application.Properties.Settings.Default.Dv2_DEBUG)
            {
                Dv2Instance.Connect();
            }
            #endregion

        }

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
            tobeRead.SpeakAsync("キーワードについて、大文字と小文字は区別されていません。");
            #endregion

            //PrimaryScreen Size
            int iActulaWidth = Screen.PrimaryScreen.Bounds.Width;
            int iActulaHeight = Screen.PrimaryScreen.Bounds.Height;
            LogOutput("PrimaryScreen Size is  " + iActulaWidth + " : " + iActulaHeight);
        }

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

        internal void codeOutput(object log)
        {
            //FontSizeノーマルは"9"
            textBox_code.Font = new Font(textBox_code.Font.FontFamily, FontSize);
            textBox_code.AppendText(log + "\r\n");
            textBox_code.ScrollToCaret();
            //Write the log file
            DV2SysLogger.Info(log);
        }

        internal void codeOutput(object log, int sub_FontSize)
        {
            //FontSizeノーマルは"9"
            textBox_code.Font = new Font(textBox_code.Font.FontFamily, sub_FontSize);
            textBox_code.AppendText(log + "\r\n");
            textBox_code.ScrollToCaret();
            //Write the log file
            DV2SysLogger.Info(log);
        }

        private void tabControl_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Debug 
            //MessageBox.Show("tabControl_code_SelectedIndexChanged");
            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.ScrollToCaret();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializationBaseDatas()
        {
            //LogOutput("Progress in Initialization Base Datas...");
            int loop_i = 0; //文字種表設定用

            for (loop_i = 0; loop_i < 256; loop_i++) { ctyp[loop_i] = TknKind.Others; }
            for (loop_i = '0'; loop_i <= '9'; loop_i++) { ctyp[loop_i] = TknKind.Digit; }
            for (loop_i = 'A'; loop_i <= 'Z'; loop_i++) { ctyp[loop_i] = TknKind.Letter; }
            for (loop_i = 'a'; loop_i <= 'z'; loop_i++) { ctyp[loop_i] = TknKind.Letter; }
            ctyp['('] = TknKind.Lparen; ctyp[')'] = TknKind.Rparen;
            ctyp['['] = TknKind.Lbracket; ctyp[']'] = TknKind.Rbracket;
            ctyp['<'] = TknKind.Less; ctyp['>'] = TknKind.Great;
            ctyp['+'] = TknKind.Plus; ctyp['-'] = TknKind.Minus;
            ctyp['*'] = TknKind.Multi; ctyp['/'] = TknKind.Divi;
            ctyp['_'] = TknKind.Letter; ctyp['='] = TknKind.Assign;
            ctyp[','] = TknKind.Comma; ctyp['"'] = TknKind.DblQ;
            ctyp['%'] = TknKind.Mod; ctyp['$'] = TknKind.Doll;
            ctyp['!'] = TknKind.Not; ctyp['?'] = TknKind.Ifsub;
            ctyp['\\'] = TknKind.IntDivi; ctyp[':'] = TknKind.Colon;

            KeyWdTbl[0] = new KeyWord("if", TknKind.If);
            KeyWdTbl[1] = new KeyWord("else", TknKind.Else);
            KeyWdTbl[2] = new KeyWord("end", TknKind.End);
            KeyWdTbl[3] = new KeyWord("print", TknKind.Print);
            KeyWdTbl[4] = new KeyWord("(", TknKind.Lparen);
            KeyWdTbl[5] = new KeyWord(")", TknKind.Rparen);
            KeyWdTbl[6] = new KeyWord("+", TknKind.Plus);
            KeyWdTbl[7] = new KeyWord("-", TknKind.Minus);
            KeyWdTbl[8] = new KeyWord("*", TknKind.Multi);
            KeyWdTbl[9] = new KeyWord("/", TknKind.Divi);
            KeyWdTbl[10] = new KeyWord("=", TknKind.Assign);
            KeyWdTbl[11] = new KeyWord(",", TknKind.Comma);
            KeyWdTbl[12] = new KeyWord("==", TknKind.Equal);
            KeyWdTbl[13] = new KeyWord("!=", TknKind.NotEq);
            KeyWdTbl[14] = new KeyWord("<", TknKind.Less);
            KeyWdTbl[15] = new KeyWord("<=", TknKind.LessEq);
            KeyWdTbl[16] = new KeyWord(">", TknKind.Great);
            KeyWdTbl[17] = new KeyWord(">=", TknKind.GreatEq);
            KeyWdTbl[18] = new KeyWord(";", TknKind.END_line);
            //New Define
            KeyWdTbl[19] = new KeyWord("[", TknKind.Lbracket);
            KeyWdTbl[20] = new KeyWord("]", TknKind.Rbracket);
            KeyWdTbl[21] = new KeyWord("!", TknKind.Not);
            KeyWdTbl[22] = new KeyWord("?", TknKind.Ifsub);
            KeyWdTbl[23] = new KeyWord("||", TknKind.Or);
            KeyWdTbl[24] = new KeyWord("&&", TknKind.And);
            KeyWdTbl[25] = new KeyWord("<-", TknKind.Assign);
            KeyWdTbl[26] = new KeyWord("to", TknKind.To);
            /* 図形命令 */
            KeyWdTbl[27] = new KeyWord("line", TknKind.Line);
            KeyWdTbl[28] = new KeyWord("arc", TknKind.Arc);
            KeyWdTbl[29] = new KeyWord("circle", TknKind.Circle);
            KeyWdTbl[30] = new KeyWord("arrow", TknKind.Arrow);
            KeyWdTbl[31] = new KeyWord("triangle", TknKind.Triangle);
            KeyWdTbl[32] = new KeyWord("rectangle", TknKind.Rectangle);
            KeyWdTbl[33] = new KeyWord("dashline", TknKind.DashLine);
            KeyWdTbl[34] = new KeyWord("dasharrow", TknKind.DashArrow);
            /* 処理関数 */
            KeyWdTbl[35] = new KeyWord("solve", TknKind.Solve); //算出
            KeyWdTbl[36] = new KeyWord("get", TknKind.Get); //指先入力点を取る
            KeyWdTbl[37] = new KeyWord("contact", TknKind.Contact); //連結
            KeyWdTbl[38] = new KeyWord("show", TknKind.Show); //表示
            KeyWdTbl[39] = new KeyWord("clear", TknKind.Clear); //削除
            KeyWdTbl[47] = new KeyWord("rotation", TknKind.Rotation); //回転
            KeyWdTbl[48] = new KeyWord("setpoint", TknKind.SetPoint); //相対位置
            /* 特別関数 */
            KeyWdTbl[40] = new KeyWord("var", TknKind.Var);
            KeyWdTbl[41] = new KeyWord(":", TknKind.Colon);
            KeyWdTbl[42] = new KeyWord("with", TknKind.With);
            KeyWdTbl[43] = new KeyWord("point", TknKind.Point);
            KeyWdTbl[44] = new KeyWord("set", TknKind.Set);
            KeyWdTbl[45] = new KeyWord("on", TknKind.On);
            KeyWdTbl[46] = new KeyWord("in", TknKind.In);
            KeyWdTbl[49] = new KeyWord("right", TknKind.Position);
            KeyWdTbl[50] = new KeyWord("left", TknKind.Position);
            KeyWdTbl[51] = new KeyWord("center", TknKind.Position);
            KeyWdTbl[52] = new KeyWord("bottom", TknKind.Position);
            KeyWdTbl[53] = new KeyWord("top", TknKind.Position);
            KeyWdTbl[54] = new KeyWord("bottomleft", TknKind.Position);
            KeyWdTbl[55] = new KeyWord("bottomright", TknKind.Position);
            KeyWdTbl[56] = new KeyWord("topleft", TknKind.Position);
            KeyWdTbl[57] = new KeyWord("topright", TknKind.Position);
            //End of the Key Word Table List Mark
            KeyWdTbl[58] = new KeyWord("", TknKind.END_keylist);
        }

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
                CameraStart();
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

        private void button_FFT_Click(object sender, EventArgs e)
        {
            // Button For Functional Test
            MakeObjectBraille();
        }

        /// <summary>
        /// ファイルの選択関数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_FS_Click(object sender, EventArgs e)
        {
            //File Selected
            if (openFileDialog_CommandFile.ShowDialog() == DialogResult.OK)
            {
                textBox_FS.Text = openFileDialog_CommandFile.FileName.ToString();
                StreamReader sR = new StreamReader(openFileDialog_CommandFile.FileName, Encoding.Default);

                //textBox_log.AppendText(sR.ReadToEnd());
                string content;
                content = sR.ReadLine();

                while (null != content)
                {
                    textBox_code.AppendText(content);
                    content = sR.ReadLine();
                }

                //textBox_code.AppendText(sR.ReadLine());
                sR.Close();
            }
        }
    }
    //End of class MainForm
}
