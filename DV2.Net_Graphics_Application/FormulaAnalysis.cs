using System;

#region Personal Addition
using System.Linq;
using System.Text.RegularExpressions;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        public void FormulaAnalysis()
        {
            //Define Parameters 
            ToKen token;
            TknKind bef_tok_kind = TknKind.None;
            loopInfo = 0;
            //DV2_Drawing dv2d_FA = new DV2_Drawing();
            string objAnalysisData = "";
            string objCommandData = "";
            string[] storageData;
            int dataGridView_index = 0;
            int objFinder_index = 0;
            #region Boolean Flags
            bool def_point = false;
            bool get_point = false;
            bool solve_point = false;
            #endregion

            //Debug
            LogOutput("Resources.Graph_line  :> " + DV2.Net_Graphics_Application.Properties.Resources.Graph_line);
            //LogOutput("Settings.GraphicInstruction  :> " + DV2.Net_Graphics_Application.Properties.Settings.Default.GraphicInstruction);
            LogOutput(dataStorage.Text.Length);

            if (dataStorage.Text.Length != 0)
            {

                LogOutput(String.Format("{0, -15}", "Text") + String.Format("{0, -15}", "Kind") + String.Format("{0, -15}", "numVal"));
                LogOutput("---------------------------------------------");

                for (char txtChar = ' '; loopInfo < dataStorage.Text.Length;)
                {

                    txtChar = dataStorage.Text[loopInfo];
                    token = nextTkn(txtChar);
                    LogOutput(">>>token.kind iS -> " + String.Format("{0, -15}", token.kind));
                    //LogOutput(">>bef_tok_kind iS -> " + String.Format("{0, -15}", bef_tok_kind));

                    if (token.kind == TknKind.END_line)
                    {
                        break;
                    }

                    //Command "Assign" Route
                    if (token.kind == TknKind.Assign)
                    {
                        //例:赋值语句 obj1 = line(0,0,20,10)
                        if (bef_tok_kind == TknKind.Ident)
                        {
                            //dataStorage
                            storageData = Regex.Split(dataStorage.Text, "=", RegexOptions.IgnoreCase);
                            //LogOutput("TknKind.Ident+TknKind.Assign Length ->" + storageData.Length);
                            //入力した命令を保存する
                            if (storageData.Length == 2)
                            {
                                ObjName.Add(storageData[0].Replace(" ", ""));
                                ObjCommand.Add(storageData[1].Replace(" ", ""));
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

                    //Command "Show" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //表示命令を判別する
                        if (bef_tok_kind == TknKind.Show)
                        {
                            tabControl_Graphics.SelectTab(1);

                            foreach (string finder in ObjName)
                            {
                                if (finder == token.text)
                                {
                                    break;
                                }
                                objFinder_index += 1;
                            }

                            if (ObjName[objFinder_index] != null && ObjAnalysis[objFinder_index] != null)
                            {
                                //LogOutput(ObjCommand[index]);
                                //LogOutput(ObjAnalysis[index]);
                                //分析結果を転送する
                                ParameterChecker(ObjAnalysis[objFinder_index], objFinder_index);
                            }
                            else
                            {
                                tobeRead.SpeakAsync(token.text + "対象は不存在，もう一度確認してください。");
                                codeOutput(token.text + "対象は不存在，もう一度確認してください。");
                            }
                        }

                    }

                    //"Point Define" Route
                    if (token.kind == TknKind.Point)
                    {
                        //変数ｐをPoint型として宣言
                        if (bef_tok_kind == TknKind.Colon)
                        {
                            //Ex:"var p : Point" -> "p = Point(0,0)" 
                            storageData = Regex.Split(dataStorage.Text, ":", RegexOptions.IgnoreCase);
                            ObjName.Add(storageData[0]);
                            ObjCommand.Add(storageData[1].Replace(" ", ""));
                            //データ監視器
                            dataGridView_index = this.dataGridView_monitor.Rows.Add();
                            this.dataGridView_monitor.Rows[dataGridView_index].Cells[0].Value = storageData[0];
                            this.dataGridView_monitor.Rows[dataGridView_index].Cells[1].Value = storageData[1].Replace(" ", "");
                            def_point = true;
                        }
                    }

                    //"Get P" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //To get the finger point on the object
                        //Ex:get p on obj1
                        if (bef_tok_kind == TknKind.Get)
                        {
                            get_point = true;
                        }
                    }

                    //"Slove" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //Slove the math program
                        //Ex:solve c by contact(obj1,obj2,p)
                        if (bef_tok_kind == TknKind.Solve)
                        {
                            solve_point = true;
                        }
                    }

                    //"Object Plus" Route
                    if (token.kind == TknKind.Ident)
                    {
                        //obj3=obj1+obj2
                        if (bef_tok_kind == TknKind.Plus)
                        {

                        }
                    }

                    //"Set" Route
                    if (token.kind == TknKind.Set)
                    {
                        //Setting the parameters

                    }

                    bef_tok_kind = token.kind;
                    objAnalysisData += token.kind + "|";
                    objCommandData += token.text + "|";
                    //LogOutput(String.Format("{0, -15}", token.text) + String.Format("{0, -15}", token.kind) + String.Format("{0, -15}", token.dblVal));
                }

                if (ObjAnalysis.Count < ObjName.Count)
                {
                    bool assignFlag = false;
                    int loop_i;
                    //分析結果を保存する
                    //LogOutput(objAnalysisData);
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
            }

            else
            {
                codeOutput("Error @FormulaAnalysis MSG: Please Input Data!");
            }

            //此処から処理するのデータは自動保存されていない。
            if (dataStorage.Text.Length != 0 && ObjName.Count != 0)
            {
                //未完成、この部分は対象名の重複データをチェックする
                DuplicateChecking();
            }

            if (def_point)
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
                    ObjCommand.Add("point(0,0)");
                    ObjAnalysis.Add("Point|Lparen|IntNum|Comma|IntNum|Rparen");
                    //データ監視器 Rewirte
                    this.dataGridView_monitor.Rows[dataGridView_index].Cells[0].Value = temp[1];
                    this.dataGridView_monitor.Rows[dataGridView_index].Cells[1].Value = "point(0,0)";
                    this.dataGridView_monitor.Rows[dataGridView_index].Cells[2].Value = "Point|Lparen|IntNum|Comma|IntNum|Rparen";
                }
            }

            if (get_point)
            {
                //get P on obj1
                //objCommandData	"get|p|on|obj1|"
                //objAnalysisData	"Get|Ident|On|Ident|"
                LogOutput("For Debug");
                objCommandData = objCommandData.Substring(0, objCommandData.Length - 1);
                objAnalysisData = objAnalysisData.Substring(0, objAnalysisData.Length - 1);

                if (Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase).Count() == 4)
                { 
                    GetPointOnObject(objCommandData, objAnalysisData);
                }
                else
                {
                    //Error
                    codeOutput("Error @FormulaAnalysis 256");
                    tobeRead.SpeakAsync("入力ミスが発生していた。");
                }
            }

            if (solve_point)
            {
                //solve c by contact(obj1,ojb2,p)
                //objCommandData	"solve|c|by|contact|(|obj1|,|ojb2|,|p|)|"
                //objAnalysisData	"Solve|Ident|Ident|Contact|Lparen|Ident|Comma|Ident|Comma|Ident|Rparen|"
                LogOutput("For Debug");
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
                LogOutput("不正なトークンです: " + txtStr);
                tobeRead.SpeakAsync("不正なトークンが存在しています.");
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
    }
}
