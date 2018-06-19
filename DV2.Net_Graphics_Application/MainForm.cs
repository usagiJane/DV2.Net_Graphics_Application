using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#region Appended
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;

using System.Speech.Recognition;
using System.Speech.Synthesis;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm : Form
    {
        #region ObjStorage
        ArrayList ObjName = new ArrayList();
        ArrayList ObjCommand = new ArrayList();
        ArrayList ObjAnalysis = new ArrayList();

        static Graphics graphObj;
        static Bitmap debug_Image;
        private static SpeechSynthesizer tobeRead = new SpeechSynthesizer();
        //FontSizeノーマルは"9"
        private static int FontSize = 9;
        //To storage the TabPages
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
            Slove, Get, Contact, Show, Clear,
            /* 特別関数 */
            With, Point, Set,
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
        KeyWord[] KeyWdTbl = new KeyWord[50];      //要約語と記号の種別対応表
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
        }

        private void formloader(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable("GraphicsDataType");
            dataTable.Columns.Add("Number", typeof(String));
            dataTable.Columns.Add("GraphicsType", typeof(String));
            dataTable.Rows.Add(new String[] { "1", "[;]" });
            dataTable.Rows.Add(new String[] { "2", @"[\r\n]" });
            dataTable.Rows.Add(new String[] { "3", @"[\n]" });

            //textBox1.Text = "this is text box 1"; be used to storage input data ->dataStorage
            //Debug
            //textBox2.Text = "this is text box 2";
            textBox3.Text = "this is text box 3";

            //Debug Mode Default Value
            //"line(1,1,100,100)||line(30,55,200,255);\r\narc(45,10,170,165,45,45);\r\ncircle(110,150,90,120);\r\narrow(25,25,230,25);\r\ntriangle(90,85,110,125,60,75);";
            //textBox_Input.Text = "obj1=line(1,2,20.0,25.5)";
            //textBox_Input.Text = "obj1=circle(1,2,20.0)";
            //textBox_Input.Text = "obj1=circle(c,20.0)";
            //textBox_Input.Text = "var p : Point";
            textBox_Input.Text = "get p on obj1";

            //DV2_Debug
            //DV2_Drawing dv2d = new DV2_Drawing();
            //dv2d.Dv2ConnectFunction(this);
            tabControl_Graphics.Visible = false;
            textBox_Input.Focus();
            //this.dataGridView_monitor.Rows.Add();
            InitializationBaseDatas();
            //
            DebugImshow();
            //Speaker
            //tobeRead.SelectVoice("Microsoft Anna");
            tobeRead.SpeakAsync("キーワードについて、大文字と小文字は区別されていません。");
        }

        internal void LogOutput(Object log)
        {
            //FontSizeノーマルは"9"
            textBox_log.Font = new Font(textBox_log.Font.FontFamily, FontSize);
            textBox_log.AppendText(log + "\r\n");
            //textBox_log.ScrollToCaret(); 
        }

        internal void codeOutput(Object log)
        {
            //FontSizeノーマルは"9"
            textBox_code.Font = new Font(textBox_code.Font.FontFamily, FontSize);
            textBox_code.AppendText(log + "\r\n");
            textBox_code.ScrollToCaret();
        }

        private void tabControl_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Debug 
            //MessageBox.Show("tabControl_code_SelectedIndexChanged");
            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.ScrollToCaret();
        }

        private void InitializationBaseDatas()
        {
            LogOutput("Progress in Initialization Base Datas...");
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
            KeyWdTbl[35] = new KeyWord("slove", TknKind.Slove);
            KeyWdTbl[36] = new KeyWord("get", TknKind.Get);
            KeyWdTbl[37] = new KeyWord("contact", TknKind.Contact);
            KeyWdTbl[38] = new KeyWord("show", TknKind.Show);
            KeyWdTbl[39] = new KeyWord("clear", TknKind.Clear);
            /* 特別関数 */
            KeyWdTbl[40] = new KeyWord("var", TknKind.Var);
            KeyWdTbl[41] = new KeyWord(":", TknKind.Colon);
            KeyWdTbl[42] = new KeyWord("with", TknKind.With);
            KeyWdTbl[43] = new KeyWord("point", TknKind.Point);
            KeyWdTbl[44] = new KeyWord("set", TknKind.Set);
            //End of the Key Word Table List Mark
            KeyWdTbl[45] = new KeyWord("", TknKind.END_keylist);
        }

        public void DebugImshow()
        {
            tabControl_Graphics.Visible = true;
            //Analog_signal_switch -> False
            if (DV2.Net_Graphics_Application.Properties.Settings.Default.Analog_signal_switch)
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

        private void ParameterChecker(object GraphIns, int listIndex)
        {
            //Debug
            LogOutput("\r\n" + "***ParameterChecker Strat!###");
            LogOutput("***object GraphIns is!###  " + GraphIns);
            //Define
            //DV2_Drawing dv2Draw = new DV2_Drawing();
            var GraphicInstruction = DV2.Net_Graphics_Application.Properties.Settings.Default.GraphicInstruction;
            string GraphCmd = Convert.ToString(GraphIns);
            string temp_ObjCom, temp_ObjAna;
            GraphCmd = Regex.Split(GraphCmd, @"\|", RegexOptions.IgnoreCase)[0];

            //Debug GraphCmd
            LogOutput("@ParameterChecker  ># " + GraphCmd + " #<");
            
            if (!Regex.IsMatch(GraphicInstruction, GraphCmd, RegexOptions.IgnoreCase))
            {
                codeOutput("Error @ParameterChecker @587");
            }

            switch (GraphCmd)
            {
                case "Line":
                    temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                    temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                    //Debug
                    LogOutput("switch GraphCmd Line -- " + temp_ObjCom);
                    LogOutput("switch GraphCmd Line -- " + temp_ObjAna);
                    
                    Draw_LineMode(temp_ObjCom, temp_ObjAna);
                    break;
                case "DashLine":
                    temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                    temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                    //Debug
                    LogOutput("switch GraphCmd DashLine -- " + temp_ObjCom);
                    LogOutput("switch GraphCmd DashLine -- " + temp_ObjAna);
                    
                    Draw_LineMode(temp_ObjCom, temp_ObjAna);
                    break;
                case "Arc":
                    break;
                case "Circle":
                    temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                    temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                    //Debug
                    LogOutput("switch GraphCmd Circle -- " + temp_ObjCom);
                    LogOutput("switch GraphCmd Circle -- " + temp_ObjAna);
                    
                    Draw_CircleMode(temp_ObjCom, temp_ObjAna);
                    break;
                case "Arrow":
                    temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                    temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                    //Debug
                    LogOutput("switch GraphCmd Arrow -- " + temp_ObjCom);
                    LogOutput("switch GraphCmd Arrow -- " + temp_ObjAna);
                    
                    Draw_ArrowMode(temp_ObjCom, temp_ObjAna);
                    break;
                case "DashArrow":
                    temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                    temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                    //Debug
                    LogOutput("switch GraphCmd DashArrow -- " + temp_ObjCom);
                    LogOutput("switch GraphCmd DashArrow -- " + temp_ObjAna);
                    
                    Draw_ArrowMode(temp_ObjCom, temp_ObjAna);
                    break;
                case "Triangle":
                    break;
                case "Rectangle":
                    break;
                case "Point":
                    break;
                default:
                    codeOutput("Error @ParameterChecker 644");
                    break;
            }
        }

        private Graphics PreparePaper()
        {
            //この関数は出力画像を準備する
            Size picSize = picBox.Size;
            debug_Image = new Bitmap(picSize.Width, picSize.Height);
            //Pen picPen = new Pen(Color.LightBlue, 2.7F);
            Graphics graphObj = Graphics.FromImage(debug_Image);

            LogOutput("picSize.Width: " + picSize.Width);
            LogOutput("picSize.Height: " + picSize.Height);
            LogOutput("comboBox Pictype SelectedIndex: " + comboBox_codeType.SelectedIndex);
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
                if (textBox2.Text.Length <= 0)
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
        /// Button Thread Tester
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            textBox_log.Font = new Font(textBox_log.Font.FontFamily, FontSize);
            textBox_log.AppendText("TEST" + "\r\n");
        }

        public void DuplicateChecking()
        {
            //未完成、この部分は対象名の重複データをチェックする
        }
    }

}
