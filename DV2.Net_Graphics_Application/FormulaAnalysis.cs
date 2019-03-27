using System;

#region Personal Addition
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        #region About Define ToKen
        /// <summary>
        /// トークン要素の集合
        /// enumは列挙型
        /// 
        /// </summary>
        public enum TknKind
        {                             /* トークン要素 */
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
            Line, DashLine, ExLine, Arrow, DashArrow, ExArrow, Arc, Circle, Triangle, Rectangle,
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

        /// <summary>
        /// 文字データ初期化
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
            KeyWdTbl[35] = new KeyWord("exarrow", TknKind.ExArrow);
            KeyWdTbl[36] = new KeyWord("exline", TknKind.ExLine);
            /* 処理関数 */
            KeyWdTbl[37] = new KeyWord("solve", TknKind.Solve); //算出
            KeyWdTbl[38] = new KeyWord("get", TknKind.Get); //指先座標点を取る
            KeyWdTbl[39] = new KeyWord("contact", TknKind.Contact); //連結
            KeyWdTbl[40] = new KeyWord("show", TknKind.Show); //表示
            KeyWdTbl[41] = new KeyWord("clear", TknKind.Clear); //削除
            KeyWdTbl[42] = new KeyWord("rotation", TknKind.Rotation); //回転
            KeyWdTbl[43] = new KeyWord("setpoint", TknKind.SetPoint); //相対位置
            /* 特別関数 */
            KeyWdTbl[44] = new KeyWord("var", TknKind.Var);
            KeyWdTbl[45] = new KeyWord(":", TknKind.Colon);
            KeyWdTbl[46] = new KeyWord("with", TknKind.With);
            KeyWdTbl[47] = new KeyWord("point", TknKind.Point);
            KeyWdTbl[48] = new KeyWord("set", TknKind.Set);
            KeyWdTbl[49] = new KeyWord("on", TknKind.On);
            KeyWdTbl[50] = new KeyWord("in", TknKind.In);
            KeyWdTbl[51] = new KeyWord("right", TknKind.Position);
            KeyWdTbl[52] = new KeyWord("left", TknKind.Position);
            KeyWdTbl[53] = new KeyWord("center", TknKind.Position);
            KeyWdTbl[54] = new KeyWord("bottom", TknKind.Position);
            KeyWdTbl[55] = new KeyWord("top", TknKind.Position);
            KeyWdTbl[56] = new KeyWord("bottomleft", TknKind.Position);
            KeyWdTbl[57] = new KeyWord("bottomright", TknKind.Position);
            KeyWdTbl[58] = new KeyWord("topleft", TknKind.Position);
            KeyWdTbl[59] = new KeyWord("topright", TknKind.Position);
            //End of the Key Word Table List Mark
            KeyWdTbl[60] = new KeyWord("", TknKind.END_keylist);
        }

        /// <summary>
        /// 入力した内容を解析するアルゴリズム
        /// </summary>
        public void FormulaAnalysis()
        {
            //Define Parameters 
            ToKen token;
            TknKind bef_tok_kind = TknKind.None;
            loopInfo = 0;
            //DV2_Drawing dv2d_FA = new DV2_Drawing();
            string objName = "";
            string objAnalysisData = "";
            string objCommandData = "";
            string[] storageData;
            int dataGridView_index = 0;
            int objFinder_index = 0;
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            #region Boolean Flags
            bool pointDef_flg = false;
            bool pointGet_flg = false;
            bool pointSol_flg = false;
            bool objPlus_flg = false;
            bool rotation_flg = false;
            bool setPoint_flg = false;
            bool clear_flg = false;
            bool cleartar_flg = false;
            #endregion

            //Debug
            //LogOutput("Resources.Graph_line  :> " + Properties.Resources.Graph_line);
            LogOutput("Formula Analysis Strat at ->   *******  " + currentTime + "  *******");
            //LogOutput("Settings.GraphicInstruction  :> " + DV2.Net_Graphics_Application.Properties.Settings.Default.GraphicInstruction);
            //LogOutput(dataStorage.Text.Length);

            if (dataStorage.Text.Length != 0)
            {

                LogOutput(String.Format("{0, -15}", "Text") + String.Format("{0, -15}", "Kind") + String.Format("{0, -15}", "numVal"));
                LogOutput("---------------------------------------------");

                #region コマンド内容はチャ－の単位として循環処理
                for (char txtChar = ' '; loopInfo < dataStorage.Text.Length;)
                {

                    txtChar = dataStorage.Text[loopInfo];
                    token = nextTkn(txtChar);
                    LogOutput(">>>token.kind iS -> " + String.Format("{0, -15}", token.kind));
                    //LogOutput(">>bef_tok_kind iS -> " + String.Format("{0, -15}", bef_tok_kind));

                    #region コマンド終わり判断
                    if (token.kind == TknKind.END_line)
                    {
                        break;
                    }
                    #endregion

                    #region Command "Assign" Route
                    if (token.kind == TknKind.Assign)
                    {
                        //例:対象を定義する　obj1 = line(0,0,20,10)
                        if (bef_tok_kind == TknKind.Ident)
                        {
                            //dataStorage
                            storageData = Regex.Split(dataStorage.Text, "=", RegexOptions.IgnoreCase);
                            //LogOutput("TknKind.Ident+TknKind.Assign Length ->" + storageData.Length);
                            //入力した命令を保存する
                            if (storageData.Length == 2)
                            {
                                if (ObjectFinder(storageData[0].Replace(" ", "")) == -1)
                                {
                                    ObjName.Add(storageData[0].Replace(" ", ""));
                                    ObjCommand.Add(storageData[1].Replace(" ", ""));
                                }
                                else
                                {
                                    objName = storageData[0].Replace(" ", "");
                                    ObjCommand[ObjectFinder(storageData[0].Replace(" ", ""))] = storageData[1].Replace(" ", "");
                                }
                                //データ監視器--データ導入
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
                    #endregion

                    #region Command "Show" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //表示命令を判別する
                        if (bef_tok_kind == TknKind.Show)
                        {
                            //tabControl_Graphics.SelectTab(1);
                            //表示したい対象名が存在するかどうかを判断する
                            if (ObjectFinder(token.text) != -1)
                            {
                                objFinder_index = ObjectFinder(token.text);
                            }
                            else
                            {
                                tobeRead.SpeakAsync("表示される " + token.text + " 対象は不存在，もう一度確認してください。");
                                codeOutput("表示される " + token.text + " 対象は不存在，もう一度確認してください。");
                                return;
                            }

                            //表示したい対象名が存在する場合は，その同じ順番配列データの有効性を判断する
                            if (ObjName[objFinder_index] != null && ObjAnalysis[objFinder_index] != null)
                            {
                                //LogOutput(ObjCommand[objFinder_index]);
                                //LogOutput(ObjAnalysis[objFinder_index]);
                                //分析結果を転送する
                                ParameterChecker(ObjAnalysis[objFinder_index], objFinder_index);
                                picBox.Refresh();
                            }

                            else
                            {
                                tobeRead.SpeakAsync("表示される " + token.text + " 対象は不存在，もう一度確認してください。");
                                codeOutput("表示される " + token.text + " 対象は不存在，もう一度確認してください。");
                            }

                            if(!ROTATIONFLAG)
                            {
                                LogOutput("ROTATIONFLAG is " + ROTATIONFLAG);
                                debug_Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                picBox.Refresh();
                                ROTATIONFLAG = true;
                            }
                            MakeObjectBraille();
                        }

                    }
                    #endregion

                    #region Command "Point Define" Route
                    if (token.kind == TknKind.Point)
                    {
                        //変数ｐをPoint型として宣言
                        if (bef_tok_kind == TknKind.Colon)
                        {
                            //Ex:"var p : Point" -> "p = Point(0,0)"  -- このコマンドは点pを原点に初期値(0,0)に設定する
                            storageData = Regex.Split(dataStorage.Text, ":", RegexOptions.IgnoreCase);
                            if (ObjectFinder(storageData[0].Replace(" ", "")) == -1)
                            {
                                ObjName.Add(storageData[0].Replace(" ", ""));
                                ObjCommand.Add(storageData[1].Replace(" ", ""));
                            }
                            else
                            {
                                objName = storageData[0].Replace(" ", "");
                                ObjCommand[ObjectFinder(storageData[0].Replace(" ", ""))] = storageData[1].Replace(" ", "");
                            }

                            //データ監視器--データ導入
                            dataGridView_index = this.dataGridView_monitor.Rows.Add();
                            this.dataGridView_monitor.Rows[dataGridView_index].Cells[0].Value = storageData[0];
                            this.dataGridView_monitor.Rows[dataGridView_index].Cells[1].Value = storageData[1].Replace(" ", "");
                            //pointDef_flgフラグをtrueに設定する
                            pointDef_flg = true;
                        }
                    }
                    #endregion

                    #region Command "Get P" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //To get the finger point on the object
                        //Ex:get p on obj1
                        if (bef_tok_kind == TknKind.Get)
                        {
                            //pointGet_flgフラグをtrueに設定する
                            pointGet_flg = true;
                        }
                    }
                    #endregion

                    #region Command "Slove" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //Slove the math program
                        //Ex:solve c by contact(obj1,obj2,p)
                        if (bef_tok_kind == TknKind.Solve)
                        {
                            //pointSol_flgフラグをtrueに設定する
                            pointSol_flg = true;
                        }
                    }
                    #endregion

                    #region Command "Object Plus" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //Ex:obj3=obj1+obj2
                        if (bef_tok_kind == TknKind.Plus)
                        {
                            //objPlus_flgフラグをtrueに設定する
                            objPlus_flg = true;
                        }
                    }
                    #endregion

                    #region Command "Clear Object" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //Ex:clear obj
                        if (bef_tok_kind == TknKind.Clear)
                        {
                            LogOutput("Clear The graphObj");
                            //cleartar_flgフラグをtrueに設定する
                            cleartar_flg = true;
                        }
                    }
                    #endregion

                    #region Command "Clear ALL" Route
                    if (token.kind == TknKind.Clear)
                    {
                        //Ex:clear
                        if (dataStorage.Text.Length != 0 && dataStorage.Text.ToLower() == "clear")
                        {
                            LogOutput("Clear ALL The graphObj");
                            //clear_flgフラグをtrueに設定する
                            clear_flg = true;
                        }
                    }
                    #endregion

                    #region Command "Set" Route
                    if (token.kind == TknKind.Set)
                    {
                        //Setting the parameters

                    }
                    #endregion

                    #region Command "Rotate" Route
                    if (token.kind == TknKind.Lparen)
                    {
                        if (bef_tok_kind == TknKind.Rotation)
                        {
                            //画像を回転する
                            rotation_flg = true;
                        }
                    }
                    #endregion

                    #region Command "SetPoint" Route
                    if (token.kind == TknKind.SetPoint)
                    {
                        //相対位置処理入口
                        setPoint_flg = true;
                    }
                    #endregion

                    bef_tok_kind = token.kind;
                    objAnalysisData += token.kind + "|";
                    objCommandData += token.text + "|";
                    //LogOutput(String.Format("{0, -15}", token.text) + String.Format("{0, -15}", token.kind) + String.Format("{0, -15}", token.dblVal));
                }
                #endregion

                #region 上書き処理
                //Modify old data
                if (ObjAnalysis.Count == ObjName.Count && objName != "" && ObjectFinder(objName) != -1)
                {
                    bool assignFlag = false;
                    int loop_i;

                    //分析結果を保存する
                    //正規表現関数
                    for (loop_i = 0; loop_i < Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase).Length; loop_i++)
                    {
                        if (Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase)[loop_i] == "Assign")
                        {
                            assignFlag = true;
                            break;
                        }
                    }

                    if (assignFlag)
                    {
                        objAnalysisData = string.Join("|", Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase).Skip(loop_i + 1).ToArray());
                        ObjAnalysis[ObjectFinder(objName)] = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                        this.dataGridView_monitor.Rows[dataGridView_index].Cells[2].Value = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                    }
                    else
                    {
                        ObjAnalysis[ObjectFinder(objName)] = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                        this.dataGridView_monitor.Rows[dataGridView_index].Cells[2].Value = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                    }

                    //Ex:"obj1|=|line|(|1|,|2|,|20.0|,|25.5|)|" -> "obj1|=|line|(|1|,|2|,|20.0|,|25.5|)"
                    ObjCommand[ObjectFinder(objName)] = objCommandData.Substring(0, objCommandData.Length - 1);
                    LogOutput("Modify Debug Point");
                }
                #endregion

                #region 解析結果
                //New data appended!
                if (ObjAnalysis.Count < ObjName.Count)
                {
                    bool assignFlag = false;
                    int loop_i;

                    //分析結果を保存する
                    //正規表現関数
                    for (loop_i = 0; loop_i < Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase).Length; loop_i++)
                    {
                        if (Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase)[loop_i] == "Assign")
                        {
                            assignFlag = true;
                            break;
                        }
                    }

                    if (assignFlag)
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

                    if (ObjCommand.Count == ObjName.Count)
                    {
                        //Ex:"obj1|=|line|(|1|,|2|,|20.0|,|25.5|)|" -> "obj1|=|line|(|1|,|2|,|20.0|,|25.5|)"
                        ObjCommand.RemoveAt(ObjCommand.Count - 1);
                        ObjCommand.Add(objCommandData.Substring(0, objCommandData.Length - 1));
                    }
                }
                #endregion
            }

            else
            {
                codeOutput("Error @FormulaAnalysis MSG: Please Input Data!");
            }

            //此処から処理するのデータは自動保存されていない。
            if (dataStorage.Text.Length != 0 && ObjName.Count != 0)
            {
                //未完成
                //予定機能は対象名の重複データをチェックする
                //DuplicateChecking();
            }

            #region PointDefine Route
            if (pointDef_flg)
            {
                if (Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase)[0].ToLower() == "var")
                {
                    string[] temp = Regex.Split(ObjCommand[ObjCommand.Count - 1].ToString(), @"\|", RegexOptions.IgnoreCase);
                    //Data Remove
                    ObjName.RemoveAt(ObjName.Count - 1);
                    ObjCommand.RemoveAt(ObjCommand.Count - 1);
                    ObjAnalysis.RemoveAt(ObjAnalysis.Count - 1);

                    //New Data Append
                    ObjName.Add(temp[1]);
                    ObjCommand.Add("Point|(|0|,|0|)");
                    ObjAnalysis.Add("Point|Lparen|IntNum|Comma|IntNum|Rparen");
                    //データ監視器 Rewirte
                    this.dataGridView_monitor.Rows[dataGridView_index].Cells[0].Value = temp[1];
                    this.dataGridView_monitor.Rows[dataGridView_index].Cells[1].Value = "point(0,0)";
                    this.dataGridView_monitor.Rows[dataGridView_index].Cells[2].Value = "Point|Lparen|IntNum|Comma|IntNum|Rparen";
                }
            }
            #endregion

            #region PointGet Route
            if (pointGet_flg)
            {
                //get P on obj1
                //objCommandData	"get|p|on|obj1|"
                //objAnalysisData	"Get|Ident|On|Ident|"
                objCommandData = objCommandData.Substring(0, objCommandData.Length - 1);
                objAnalysisData = objAnalysisData.Substring(0, objAnalysisData.Length - 1);

                if (Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase).Count() == 4)
                {
                    GetPointOnObject(objCommandData, objAnalysisData);
                }
                else
                {
                    //Error
                    LogOutput("Error @FormulaAnalysis PointGet Route");
                    tobeRead.SpeakAsync("入力ミスが発生していた。");
                }
            }
            #endregion

            #region The Solve Route
            if (pointSol_flg)
            {
                //solve c by contact(obj1,ojb2,p)
                //objCommandData	"solve|c|by|contact|(|obj1|,|ojb2|,|p|)|"
                //objAnalysisData	"Solve|Ident|Ident|Contact|Lparen|Ident|Comma|Ident|Comma|Ident|Rparen|"
                objCommandData = objCommandData.Substring(0, objCommandData.Length - 1);
                objAnalysisData = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                string[] tmpCommData = Regex.Split(objCommandData, @"\|", RegexOptions.IgnoreCase);
                string[] tmpAnaData = Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase);
                int identCounter = 0;
                bool contact_flag = false;

                foreach (var temp in tmpAnaData)
                {
                    if (temp == "Ident")
                        identCounter++;
                    if (temp == "Contact")
                    {
                        contact_flag = true;
                    }
                }
                if (contact_flag == true && identCounter == 5)
                {
                    TheSolveMode(objCommandData, tmpCommData, objAnalysisData, tmpAnaData);
                }
            }
            #endregion

            #region objPlus Route
            if (objPlus_flg)
            {
                //obj3=obj1+obj2
                //objCommandData	"obj3|=|obj1|+|obj2|"
                //objAnalysisData	"Ident|Plus|Ident|"
                objCommandData = objCommandData.Substring(0, objCommandData.Length - 1);
                objAnalysisData = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                AssignRemover(ref objCommandData);
                string[] tmpCommData = Regex.Split(objCommandData, @"\|", RegexOptions.IgnoreCase);
                string[] tmpAnaData = Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase);

                if (objAnalysisData != "Ident|Plus|Ident")
                {
                    //Error Route
                    return;
                }

                for (int i = 0; i < tmpAnaData.Length; i++)
                {
                    if (tmpAnaData[i] == "Ident")
                    {
                        if (ObjectFinder(tmpCommData[i]) == -1)
                        {
                            tobeRead.SpeakAsync(ObjectFinder(tmpCommData[i]) + "対象は定義されていません！");
                            codeOutput(ObjectFinder(tmpCommData[i]) + "対象は定義されていません！");
                        }
                    }
                }
            }
            #endregion

            #region Rotation Route
            if (rotation_flg)
            {
                //rotation -30
                //objCommandData	"rotation|(|obj1|,|30|)|"
                //objAnalysisData	"Rotation|Lparen|Ident|Comma|IntNum|Rparen|"
                objCommandData = objCommandData.Substring(0, objCommandData.Length - 1);
                objAnalysisData = objAnalysisData.Substring(0, objAnalysisData.Length - 1);

                Rotation(objCommandData, objAnalysisData);
            }
            #endregion

            #region setPoint Route
            if (setPoint_flg)
            {
                //obj1:setpoint(LEFT,obj2,Right,10,0)
                //objCommandData	"obj1|:|setpoint|(|LEFT|,|obj2|,|Right|,|10|,|0|)|"
                //objAnalysisData	"Ident|Colon|SetPoint|Lparen|Left|Comma|Ident|Comma|Right|Comma|IntNum|Comma|IntNum|Rparen|"
                objCommandData = objCommandData.Substring(0, objCommandData.Length - 1);
                objAnalysisData = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                LogOutput("objCommandData     " + objCommandData);
                LogOutput("objAnalysisData    " + objAnalysisData);

                TheSetPointMode(objCommandData, objAnalysisData);
            }
            #endregion

            #region Clear Route
            if (clear_flg)
            {
                LogOutput("clear_flg" + clear_flg);
                graphObj.Dispose();
                debug_Image.Dispose();
                graphObj = PreparePaper();
                picBox.Image = (Image)debug_Image;
                picBox.Refresh();

                ObjDisplayed.Clear();
                DotDataInitialization(ref forDisDots);
                DotDataInitialization(ref allDotData);
                Dv2Instance.SetDots(forDisDots, BlinkInterval);

                if (!ROTATIONFLAG)
                {
                    LogOutput("ROTATIONFLAG is " + ROTATIONFLAG);
                    debug_Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    picBox.Refresh();
                    ROTATIONFLAG = true;
                }
            }

            if (cleartar_flg)
            {
                //clear objA
                //objCommandData   clear|objA
                //objAnalysisData  Clear|Ident
                objCommandData = objCommandData.Substring(0, objCommandData.Length - 1);
                objAnalysisData = objAnalysisData.Substring(0, objAnalysisData.Length - 1);
                LogOutput("objCommandData     " + objCommandData);
                LogOutput("objAnalysisData    " + objAnalysisData);

                if (ObjDisplayed.Count == 1 || ObjDisplayed.Count == 0)
                {
                    graphObj.Dispose();
                    debug_Image.Dispose();
                    graphObj = PreparePaper();
                    picBox.Image = (Image)debug_Image;
                    picBox.Refresh();

                    ObjDisplayed.Clear();
                    DotDataInitialization(ref forDisDots);
                    DotDataInitialization(ref allDotData);
                    Dv2Instance.SetDots(forDisDots, BlinkInterval);
                }

                else
                {
                    //Processing
                    if (!ClearTargetAndCheck(objCommandData, objAnalysisData))
                    {
                        //処理失敗の流れ

                    }
                }
            }
            #endregion

            LogOutput("---------------------------------------------");
            LogOutput("Formula Analysis End at ->   *******  " + System.DateTime.Now + "  *******");
            if (ROTATIONFLAG)
            {
                LogOutput("ROTATIONFLAG is " + ROTATIONFLAG);
                debug_Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                ROTATIONFLAG = false;
            }
        }

        #region ToKen Analysis
        /* トークン取得。漢字やエスケープ文字は非対応 */
        ToKen nextTkn(char txtChar)
        {
            TknKind kd;
            int temp_ch = 0;
            //double digitNum = 0;
            string txtStr = "";
            ToKen tokenInfo = new ToKen(TknKind.None, " ", 0.0);

            while (Char.IsWhiteSpace(txtChar))
            {
                /* 空白読み捨て */
                txtChar = nextCh();
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
                /* 識別子 */
                case TknKind.Letter:
                    for (; ctyp[txtChar] == TknKind.Letter || ctyp[txtChar] == TknKind.Digit; txtChar = nextCh())
                    {
                        txtStr += txtChar;
                    }
                    break;
                /* 数字 */
                case TknKind.Digit:
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
                /* 負数 */
                case TknKind.Minus:
                    tokenInfo.kind = TknKind.IntNum;
                    txtStr += txtChar;
                    txtChar = nextCh();
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
                    if (txtStr == "-")
                    {
                        tobeRead.SpeakAsync("減数の形は数字のみです、文字列と引数はサポートされていません。");
                        tokenInfo.kind = TknKind.Minus;
                        tokenInfo.text = txtStr;
                        return tokenInfo;
                    }
                    else
                    {
                        tokenInfo.text = txtStr;
                        tokenInfo.dblVal = Convert.ToDouble(txtStr);
                    }
                    return tokenInfo;
                /* 文字列定数 */
                case TknKind.DblQ:
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
                /* 記号 */
                default:
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
            /* 種別設定 */
            kd = get_kind(ref txtStr);
            if (kd == TknKind.Others)
            {
                LogOutput("サポートされていないトークンが存在する: " + txtStr);
                tobeRead.SpeakAsync("サポートされていないトークンが存在する.");
                //exit(1);
            }
            tokenInfo.kind = kd;
            tokenInfo.text = txtStr;
            return tokenInfo;
        }

        /* 2文字演算子なら真 */
        public bool is_ope2(int charA, int charB)
        {
            string ope2Str = " <= >= == != ";
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

            if (loopInfo < dataStorage.Text.Length)
            {
                loopInfo += 1;
                if (loopInfo == dataStorage.Text.Length)
                {
                    return '\0';
                }
                else
                {
                    nextChar = dataStorage.Text[loopInfo];
                }

                return nextChar;
            }
            else
            {
                return '\0';
            }

        }

        TknKind get_kind(ref string kindStr)
        {
            for (int i = 0; KeyWdTbl[i].keyKind != TknKind.END_keylist; i++)
            {
                if (kindStr.ToLower() == KeyWdTbl[i].keyName) return KeyWdTbl[i].keyKind;
            }
            if (ctyp[kindStr[0]] == TknKind.Letter) return TknKind.Ident;
            if (ctyp[kindStr[0]] == TknKind.Digit) return TknKind.IntNum;
            return TknKind.Others;
        }
        #endregion

        /// <summary>
        /// 重複チェック，現在未使用
        /// </summary>
        /// <param name="ObjName">対象名</param>
        public void DuplicateChecking(string ObjName)
        {
            //Define
            int objIndex;

            objIndex = ObjectFinder(ObjName);
            if(objIndex == -1)
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// パラメータチェッカー、show命令を実行する時に、呼び出し関数
        /// </summary>
        /// <param name="GraphIns"></param>
        /// <param name="listIndex"></param>
        private void ParameterChecker(object GraphIns, int listIndex)
        {
            //For Command "Show" Route
            //Debug
            LogOutput("\r\n" + "***ParameterChecker Strat!###");
            LogOutput("***object GraphIns is!###  " + GraphIns);
            //Define
            //DV2_Drawing dv2Draw = new DV2_Drawing();
            string[] GraphicInstruction = Regex.Split(Properties.Settings.Default.GraphicInstruction, @"\|", RegexOptions.IgnoreCase);
            var SpecialInstruction = Properties.Settings.Default.SpecialInstruction;
            string GraphCmd = GraphIns.ToString();
            string temp_ObjCom, temp_ObjAna;
            GraphCmd = Regex.Split(GraphCmd, @"\|", RegexOptions.IgnoreCase)[0];

            //Debug GraphCmd
            LogOutput("@ParameterChecker  ># " + GraphCmd + " #<");

            //Save Displayed Object
            //二回挿入防止
            if (DisplayedObjectFinder(ObjName[listIndex].ToString()) == -1)
            {
                ObjDisplayed.Add(ObjName[listIndex]);
            }
            
            #region GraphicInstruction
            if (GraphicInstruction.Contains(GraphCmd) )
            {
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
                    case "ExLine":
                        temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                        temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                        //Debug
                        LogOutput("switch GraphCmd ExLine -- " + temp_ObjCom);
                        LogOutput("switch GraphCmd ExLine -- " + temp_ObjAna);

                        Draw_ExLineMode(temp_ObjCom, temp_ObjAna);
                        break;
                    case "Arc":
                        temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                        temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                        //Debug
                        LogOutput("switch GraphCmd Arc -- " + temp_ObjCom);
                        LogOutput("switch GraphCmd Arc -- " + temp_ObjAna);

                        Draw_CurveMode(temp_ObjCom, temp_ObjAna);
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
                    case "ExArrow":
                        temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                        temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                        //Debug
                        LogOutput("switch GraphCmd ExArrow -- " + temp_ObjCom);
                        LogOutput("switch GraphCmd ExArrow -- " + temp_ObjAna);

                        Draw_ExArrowMode(temp_ObjCom, temp_ObjAna);
                        break;
                    case "Triangle":
                        temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                        temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                        //Debug
                        LogOutput("switch GraphCmd Triangle -- " + temp_ObjCom);
                        LogOutput("switch GraphCmd Triangle -- " + temp_ObjAna);

                        Draw_TriangleMode(temp_ObjCom, temp_ObjAna);
                        break;
                    case "Rectangle":
                        temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                        temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                        //Debug
                        LogOutput("switch GraphCmd Rectangle -- " + temp_ObjCom);
                        LogOutput("switch GraphCmd Rectangle -- " + temp_ObjAna);

                        Draw_QuadrilateralMode(temp_ObjCom, temp_ObjAna);
                        break;
                    case "Point":
                        temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                        temp_ObjAna = Convert.ToString(ObjAnalysis[listIndex]);
                        //Debug
                        LogOutput("switch GraphCmd Point -- " + temp_ObjCom);
                        LogOutput("switch GraphCmd Point -- " + temp_ObjAna);

                        Draw_PointMode(temp_ObjCom, temp_ObjAna);
                        break;
                    default:
                        codeOutput("Error @ParameterChecker @852");
                        break;
                }
            }
            #endregion

            #region SpecialInstruction
            else if (Regex.IsMatch(SpecialInstruction, GraphCmd, RegexOptions.IgnoreCase))
            {
                if (GraphCmd == "Ident")
                {
                    temp_ObjCom = Convert.ToString(ObjCommand[listIndex]);
                    AssignRemover(ref temp_ObjCom);

                    //GraphIns   Ident|Plus|Ident
                    //temp_ObjCom   obj1|+|obj2
                    for (int i = 0; i < Regex.Split(GraphIns.ToString(), @"\|", RegexOptions.IgnoreCase).Length; i++)
                    {
                        if (Regex.Split(GraphIns.ToString(), @"\|", RegexOptions.IgnoreCase)[i] == "Ident")
                        {
                            if (ObjectFinder(Regex.Split(temp_ObjCom, @"\|", RegexOptions.IgnoreCase)[i]) != -1)
                            {
                                ParameterChecker(ObjAnalysis[ObjectFinder(Regex.Split(temp_ObjCom, @"\|", RegexOptions.IgnoreCase)[i])], ObjectFinder(Regex.Split(temp_ObjCom, @"\|", RegexOptions.IgnoreCase)[i]));
                            }

                            else
                            {
                                tobeRead.SpeakAsync("Errorになっております、入力内容を確認してください。詳しい内容はlogに表示します。");
                                LogOutput("Errorになっております、入力内容を確認してください。詳しい内容はlogに表示します。");
                                LogOutput("Error @ParameterChecker -> SpecialInstruction @923");
                            }
                        }
                    }
                }
            }
            #endregion

            else
            {
                codeOutput("Error @ParameterChecker @933");
            }
        }


    //End of class @FormulaAnalysis
    }
}
