using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

#region Appended
using System.Collections;
using System.Text.RegularExpressions;

using System.Speech.Recognition;
using System.Speech.Synthesis;
#endregion

#region Description
/********************************************************************************* 
  *Copyright(C),Lab
  *FileName:
  *Author:
  *Version:
  *Date:
  *Description:
  * 
  *Others:
  *Function List:
     1.………… 
     2.………… 
  *History:
     1.Date: 
       Author: 
       Modification: 
     2.………… 
**********************************************************************************/
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
            IntDivi = '\\', Comma = ',', DblQ = '"',
            Func = 150, Var, If, Elif, Else, For, To, Step, While,
            End, Break, Return, Option, Print, Println, Input, Toint,
            Exit, Equal, NotEq, Less, LessEq, Great, GreatEq, And, Or,
            END_KeyList, 
            Ident, IntNum, DblNum, String, Letter, Doll, Digit,
            Gvar, Lvar, Fcall, EofProg, EofLine, Others, EofTkn, None, END_keylist, END_line,
            /* 図形命令 */
            Line, Arc, Circle, Arrow, Triangle, Rectangle,
            /* 処理関数 */
            Slove, Get, Contact, Show, Clear,
            /* 特別関数 */
            With, Point, 
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
        KeyWord[] KeyWdTbl = new KeyWord[44];      //要約語と記号の種別対応表
        TknKind[] ctyp = new TknKind[256];         //文字種表定義
        private int loopInfo = 0;
        #endregion

        public MainForm()
        {
            InitializeComponent();
            //紙を用意する
            graphObj = PreparePaper();

            //ComboBox データ初期化
            //String[] arr = new String[] { "ライン", "曲線", "円", "矢印" };
            String[] typeArr = new String[] { "[;]", @"[\r\n]", @"[\n]" };
            for (int i = 0; i < typeArr.Length; i++)
            {
                comboBox_codeType.Items.Add(typeArr[i]);
            }

            comboBox_codeType.SelectedIndex = 0;

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

            //comboBox_pictype.DataSource = dataTable;
            //textBox1.Text = "this is text box 1"; be used to storage input data ->dataStorage
            //Debug
            //textBox2.Text = "this is text box 2";
            textBox3.Text = "this is text box 3";

            //Debug Mode デフォルト値
            //"line(1,1,100,100)||line(30,55,200,255);\r\narc(45,10,170,165,45,45);\r\ncircle(110,150,90,120);\r\narrow(25,25,230,25);\r\ntriangle(90,85,110,125,60,75);";
            textBox_Input.Text = "obj1=line(1,2,20.0,25.5)";
            
            //DV2_Debug
            DV2_Drawing dv2d = new DV2_Drawing();
            //dv2d.Dv2ConnectFunction(this);
            tabControl_Graphics.Visible = false;
            textBox_Input.Focus();
            //this.dataGridView_monitor.Rows.Add();
            InitializationBaseDatas();
            DebugImshow();
            //Delegate 
        }

        internal void LogOutput(Object log)
        {
            //if (textBox_log.GetLineFromCharIndex(textBox_log.Text.Length) > 11)
            //{
            //    //line > 11 clean
            //    textBox_log.Text = "";
            //}
            //textBox_log.AppendText(DateTime.Now.ToString("HH:mm") + " " + log + "\r\n");

            //FontSizeノーマルは"9"
            textBox_log.Font = new Font(textBox_log.Font.FontFamily, FontSize);
            textBox_log.AppendText(log + "\r\n");
            //textBox_log.ScrollToCaret(); 
        }
        
        public void codeOutput(Object log)
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
            ctyp['\\'] = TknKind.IntDivi; //ctyp['\"'] = TknKind.DblQ;

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
            KeyWdTbl[27] = new KeyWord("var", TknKind.Var);
            /* 図形命令 */
            KeyWdTbl[28] = new KeyWord("line", TknKind.Line);
            KeyWdTbl[29] = new KeyWord("arc", TknKind.Arc);
            KeyWdTbl[30] = new KeyWord("circle", TknKind.Circle);
            KeyWdTbl[31] = new KeyWord("arrow", TknKind.Arrow);
            KeyWdTbl[32] = new KeyWord("triangle", TknKind.Triangle);
            KeyWdTbl[33] = new KeyWord("rectangle", TknKind.Rectangle);
            /* 処理関数 */
            KeyWdTbl[34] = new KeyWord("slove", TknKind.Slove);
            KeyWdTbl[35] = new KeyWord("get", TknKind.Get);
            KeyWdTbl[36] = new KeyWord("contact", TknKind.Contact);
            KeyWdTbl[37] = new KeyWord("show", TknKind.Show);
            KeyWdTbl[38] = new KeyWord("clear", TknKind.Clear);
            /* 特別関数 */
            KeyWdTbl[39] = new KeyWord("with", TknKind.With);
            KeyWdTbl[40] = new KeyWord("point", TknKind.Point);
            //End of the Key Word Table List Mark
            KeyWdTbl[41] = new KeyWord("", TknKind.END_keylist);
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
            DV2_Drawing GraphicDraw = new DV2_Drawing();

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

        #region ToKen Debug
        public void FormulaAnalysis()
        {
            //Define Parameters 
            ToKen token;
            TknKind bef_tok_kind = TknKind.None;
            loopInfo = 0;
            DV2_Drawing dv2d_FA = new DV2_Drawing();
            string objAnalysisData = "";
            string[] storageData;
            int dataGridView_index = 0;

            //Debug
            LogOutput("Resources.Graph_line :>" + DV2.Net_Graphics_Application.Properties.Resources.Graph_line);
            LogOutput("Settings.GraphicInstruction :>" + DV2.Net_Graphics_Application.Properties.Settings.Default.GraphicInstruction);

            if (dataStorage.Text.Length != 0)
            {
                
                LogOutput(String.Format("{0, -15}", "Text") + String.Format("{0, -15}", "Kind") + String.Format("{0, -15}", "numVal"));
                LogOutput("---------------------------------------------");

                for (char txtChar = ' '; loopInfo < dataStorage.Text.Length - 1;)
                {

                    txtChar = dataStorage.Text[loopInfo];
                    token = nextTkn(txtChar);
                    LogOutput(">>>token.kind iS -> " + String.Format("{0, -15}", token.kind));
                    LogOutput(">>bef_tok_kind iS -> " + String.Format("{0, -15}", bef_tok_kind));

                    if (token.kind == TknKind.END_line)
                    { 
                        break;
                    }

                    //例:赋值语句 obj1 = line(0,0,20,10)
                    if (token.kind == TknKind.Assign)
                    {
                        if (bef_tok_kind == TknKind.Ident)
                        {
                            //dataStorage
                            storageData = Regex.Split(dataStorage.Text, "=", RegexOptions.IgnoreCase);
                            //LogOutput("TknKind.Ident+TknKind.Assign Length ->" + storageData.Length);
                            //入力した命令を保存する
                            if (storageData.Length == 2)
                            {
                                ObjName.Add(storageData[0].Replace(" ", ""));
                                ObjCommand.Add(storageData[1].Replace(" ",""));
                                //データ監視器
                                dataGridView_index = this.dataGridView_monitor.Rows.Add();
                                this.dataGridView_monitor.Rows[dataGridView_index].Cells[0].Value = storageData[0].Replace(" ", "");
                                this.dataGridView_monitor.Rows[dataGridView_index].Cells[1].Value = storageData[1].Replace(" ", "");
                            }
                            else
                            {
                                codeOutput("Error @FormulaAnalysis @341");
                                break;
                            }
                        }
                    }

                    else if (token.kind == TknKind.Ident)
                    {
                        //表示命令を判別する
                        if (bef_tok_kind == TknKind.Show)
                        {
                            tabControl_Graphics.SelectTab(1);
                            if (ObjName.BinarySearch(token.text) != -1)
                            {
                                int index = ObjName.BinarySearch(token.text);
                                LogOutput(ObjCommand[index]);
                                LogOutput(ObjAnalysis[index]);
                                //分析結果を転送する
                                ParameterChecker(ObjAnalysis[index], index);
                            }
                        }
                        
                    }
                    bef_tok_kind = token.kind;
                    objAnalysisData += token.kind + "|";
                    //LogOutput(String.Format("{0, -15}", token.text) + String.Format("{0, -15}", token.kind) + String.Format("{0, -15}", token.dblVal));
                }

                if (ObjAnalysis.Count < ObjCommand.Count)
                {
                    bool flag = false;
                    int loop_i;
                    //分析結果を保存する
                    //LogOutput(objAnalysisData);

                    for (loop_i = 0; loop_i < Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase).Length; loop_i++)
                    {
                        if (Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase)[loop_i] == "Assign")
                        {
                            flag = true;
                            break;
                        }
                    }
                   
                    if (flag)
                    {
                        objAnalysisData = String.Join("|", Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase).Skip(loop_i + 1).ToArray());
                        ObjAnalysis.Add(objAnalysisData.Substring(0, objAnalysisData.Length - 1));
                        this.dataGridView_monitor.Rows[dataGridView_index].Cells[2].Value = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                    }
                    else
                    {
                        ObjAnalysis.Add(objAnalysisData.Substring(0, objAnalysisData.Length - 1));
                        this.dataGridView_monitor.Rows[dataGridView_index].Cells[2].Value = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                    }
                }
            }
            else
            {
                codeOutput("Error @FormulaAnalysis MSG: Please Input Data!");
            }

        }

        ToKen nextTkn(char txtChar)                   /* トークン取得。漢字やエスケープ文字は非対応 */
        {
            TknKind kd;
            int temp_ch = 0;
            //double digitNum = 0;
            string txtStr = "";
            ToKen tokenInfo = new ToKen(TknKind.None, " ", 0.0);
            
            while (Char.IsWhiteSpace(txtChar))
            {
                txtChar = nextCh();                   /* 空白読み捨て */
            }

            //if (ch == EOF) return Token(EofTkn, txt);
            if (txtChar == '\0' || txtChar == '\r' || txtChar == '\n')
            {
                //return ToKen(TknKind.EofTkn, txt);
                tokenInfo.kind = TknKind.EofTkn;
                tokenInfo.text = txtStr;

                return tokenInfo;
            }

            switch (ctyp[txtChar])
            {
                case TknKind.Letter:                            /* 識別子 */
                    for (; ctyp[txtChar] == TknKind.Letter || ctyp[txtChar] == TknKind.Digit; txtChar = nextCh())
                    {
                        txtStr += txtChar;
                    }
                    break;
                case TknKind.Digit:                             /* 数字 */
                    tokenInfo.kind = TknKind.IntNum;
                    while (ctyp[txtChar] == TknKind.Digit)
                    {
                        txtStr += txtChar;
                        txtChar = nextCh();
                    }
                    if (txtChar == '.')
                    {
                        tokenInfo.kind = TknKind.DblNum;
                        txtStr += txtChar;
                        txtChar = nextCh();
                    }
                    while (ctyp[txtChar] == TknKind.Digit)
                    {
                        txtStr += txtChar;
                        txtChar = nextCh();
                    }

                    tokenInfo.text = txtStr;
                    tokenInfo.dblVal = Convert.ToDouble(txtStr);
                    return tokenInfo;
                case TknKind.DblQ:                              /* 文字列定数 */
                    for (txtChar = nextCh(); txtChar != '\0' && txtChar != '\n' && txtChar != '"'; txtChar = nextCh())
                    {
                        txtStr += txtChar;
                    }
                    if (txtChar != '"')
                    {
                        LogOutput("文字列リテラルが閉じていない");
                        //exit(1);
                    }
                    txtChar = nextCh();
                    tokenInfo.kind = TknKind.String;
                    tokenInfo.text = txtStr;
                    return tokenInfo;
                case TknKind.Less:
                    {
                        txtStr += txtChar;
                        temp_ch = txtChar;
                        txtChar = nextCh();
                        if (ctyp[txtChar] == TknKind.Minus && txtChar != '\0')
                        {
                            txtStr += txtChar;
                            txtChar = nextCh();
                        }
                        else
                        {
                            if (is_ope2(temp_ch, txtChar))
                            {
                                txtStr += txtChar;
                                txtChar = nextCh();
                            }
                            break;
                        }
                    }
                    tokenInfo.kind = TknKind.Assign;
                    tokenInfo.text = txtStr;
                    return tokenInfo;
                default :                                /* 記号 */
                    {
                        txtStr += txtChar;
                        temp_ch = txtChar;
                        txtChar = nextCh();
                        if (is_ope2(temp_ch, txtChar))
                        {
                            txtStr += txtChar;
                            txtChar = nextCh();
                        }
                        break;
                    }
            }
            kd = get_kind(ref txtStr);                     /* 種別設定 */
            if (kd == TknKind.Others)
            {
                LogOutput("不正なトークンです: " + txtStr);
                tobeRead.Speak("不正なトークンが存在しています.");
                //exit(1);
            }
            tokenInfo.kind = kd;
            tokenInfo.text = txtStr;
            return tokenInfo;
        }
        
        public bool is_ope2(int charA, int charB)                /* 2文字演算子なら真 */
        {
            string ope2Str = " <= >= == != <- ";
            string chkStr;

            if (charA == '\0' || charB == '\0')
            { 
                return false;
            }
            chkStr = charA.ToString() + charB.ToString();
            bool opFlag = ope2Str.Contains(chkStr.ToString());

            if (opFlag)
            {
                return true;
            }
            else
                return false;
        }

        public char nextCh()
        {
            char nextChar;
            string textboxData = dataStorage.Text;

            if (loopInfo < dataStorage.Text.Length - 1)
            {
                loopInfo += 1;
                nextChar = dataStorage.Text[loopInfo];
                return nextChar;
            }
            else
            {
                return '\0';
            }
            
        }

        TknKind get_kind(ref string s)
        {
	        for (int i = 0; KeyWdTbl[i].keyKind != TknKind.END_keylist; i++)
            {
		        if (s == KeyWdTbl[i].keyName) return KeyWdTbl[i].keyKind;
	        }
	        if (ctyp[s[0]] == TknKind.Letter) return TknKind.Ident;
	        if (ctyp[s[0]] == TknKind.Digit)  return TknKind.IntNum;
	        return TknKind.Others;
        }
        #endregion

        private void ParameterChecker(object GraphIns, int listIndex)
        {
            //Debug
            LogOutput("\r\n" + "***ParameterChecker Strat!###");
            LogOutput("***object GraphIns is!###  " + GraphIns);
            //Define
            DV2_Drawing dv2Draw = new DV2_Drawing();
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
                    dv2Draw.Draw_LineMode(temp_ObjCom, temp_ObjAna, this);
                    break;
                case "Arc":
                    break;
                case "Circle":
                    temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                    temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                    //Debug
                    LogOutput("switch GraphCmd Circle -- " + temp_ObjCom);
                    LogOutput("switch GraphCmd Circle -- " + temp_ObjAna);
                    dv2Draw.Draw_CircleMode(temp_ObjCom, temp_ObjAna);
                    break;
                case "Arrow":
                    break;
                case "Triangle":
                    break;
                case "Rectangle":
                    break;
                case "Point":
                    break;
                default:
                    codeOutput("Error @ParameterChecker");
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
    }

}
