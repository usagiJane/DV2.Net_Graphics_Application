using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;
#endregion

namespace DV2.Net_Graphics_Application
{
    //class Graphic_QuadrilateralClass　四辺形
    public partial class MainForm
    {
        /// <summary>
        /// 矩形を描く関数の入口，命令文と字句解析結果を入力すれば，矩形を描く
        /// </summary>
        /// <param name="ObjComm"></param>
        /// <param name="ObjAna"></param>
        public void Draw_QuadrilateralMode(string ObjComm, string ObjAna)
        {
            //Debug
            LogOutput("Draw_QuadrilateralMode");
            //LogOutput("Parameter 1  -> " + ObjComm);
            //LogOutput("Parameter 2  -> " + ObjAna + "\r\n");
            //Parameter 1->obj1 |=| Rectangle | (| c |,| 60 |,| 60 |)
            //Parameter 2->Rectangle | Lparen | Ident | Comma | IntNum | Comma | IntNum | Rparen

            //Define
            string[] chkData, commData, poiCom, poiAna;
            string backObjComm = ObjComm, tmpCom;
            ArrayList pointData = new ArrayList();
            Pen picPen = new Pen(Color.Black, 0.1F);
            bool isIdent = false;
            int loopCount = 0;

            //Processing
            AssignRemover(ref ObjComm);
            commData = Regex.Split(ObjComm, @"\|", RegexOptions.IgnoreCase);
            chkData = Regex.Split(ObjAna, @"\|", RegexOptions.IgnoreCase);
            //ObjComm	"Rectangle|(|c|,|60|,|60|)"
            //ObjAna	"Rectangle|Lparen|Ident|Comma|IntNum|Comma|IntNum|Rparen"

            for (int i = 0; i < chkData.Length; i++)
            {
                if (chkData[i] == "Ident")
                {
                    isIdent = true;
                    break;
                }
                loopCount++;
            }

            if(isIdent)
            {
                //定位ポイント
                if(ObjectFinder(commData[loopCount]) != -1)
                {
                    //ObjectFinder(commData[loopCount])
                    tmpCom = ObjCommand[ObjectFinder(commData[loopCount])].ToString();
                    AssignRemover(ref tmpCom);
                    poiCom = Regex.Split(tmpCom, @"\|", RegexOptions.IgnoreCase);
                    poiAna = Regex.Split(ObjAnalysis[ObjectFinder(commData[loopCount])].ToString(), @"\|", RegexOptions.IgnoreCase);

                    for (int i = 0; i < poiAna.Length; i++)
                    {
                        if (poiAna[i] == "IntNum" || poiAna[i] == "DblNum")
                        {
                            pointData.Add(poiCom[i]);
                        }
                    }
                    for (int i = 0; i < chkData.Length; i++)
                    {
                        if (chkData[i] == "IntNum" || chkData[i] == "DblNum")
                        {
                            pointData.Add(commData[i]);
                        }
                    }
                }
                else
                {
                    tobeRead.SpeakAsync("座標点は見つからない、もう一度やり直してください。");
                    LogOutput("座標点は見つからない、もう一度やり直してください。");
                    return;
                }
            }
            else
            {
                for (int i = 0; i < chkData.Length; i++)
                {
                    if (chkData[i] == "IntNum" || chkData[i] == "DblNum")
                    {
                        pointData.Add(commData[i]);
                    }
                }
            }

            //Function [DrawQuadrilateral] Call
            if (commData[0].ToLower() == "rectangle" && chkData[0] == "Rectangle" && pointData.Count == 4)
            {
                DrawQuadrilateral(ref pointData, picPen);
            }
            else if (commData[0].ToLower() == "dashrectangle" && chkData[0] == "DashRectangle" && pointData.Count == 4)
            {
                DrawQuadrilateral(ref pointData, picPen, true);
            }
            else if (commData[0].ToLower() == "fillrectangle" && chkData[0] == "FillRectangle" && pointData.Count == 4)
            {
                DrawQuadrilateral(ref pointData, picPen, false, true);
            }
            else
            {
                tobeRead.SpeakAsync("@Graphic_QuadrilateralClass Draw_QuadrilateralMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
                codeOutput("@Graphic_QuadrilateralClass Draw_QuadrilateralMode関数" + ObjName[ObjectCommandFinder(backObjComm)] + "対象定義識別失敗!");
            }
        }

        /// <summary>
        /// 実際に矩形を描く関数
        /// </summary>
        /// <param name="pointData">線分データ群</param>
        /// <param name="picPen">線分の色と太さ</param>
        /// <param name="dashFlag">破線フラグ</param>
        /// <param name="fillFlag">塗りつぶしフラグ</param>
        /// <param name="offset">変位量</param>
        private void DrawQuadrilateral(ref ArrayList pointData, Pen picPen = null, bool dashFlag = false, bool fillFlag = false, float offset = pub_offSet)
        {
            LogOutput("DrawQuadrilateral (" + pointData[0] + "," + pointData[1] + ") -> (" + pointData[2] + "," + pointData[3] + ")");
            LogOutput("FocusPoint is " + Properties.Settings.Default.FocusPoint);
            //offset 変位量
            //pointData 座標点ArrayList,可変パラメータ
            picPen = picPen ?? pub_picPen;
            float pointAx = 0, pointAy = 0, width = 0, height = 0;

            #region アンカー座標計算
            if ("upperleft左上".Contains(Properties.Settings.Default.FocusPoint))
            {
                pointAx = Convert.ToSingle(pointData[0]) + offset + Point_Offset.X;
                pointAy = Convert.ToSingle(pointData[1]) + offset + Point_Offset.Y;
                width = Convert.ToSingle(pointData[2]) + offset;
                height = Convert.ToSingle(pointData[3]) + offset;
            }
            else if ("bottomleft左下".Contains(Properties.Settings.Default.FocusPoint))
            {
                pointAx = Convert.ToSingle(pointData[0]) + offset + Point_Offset.X;
                pointAy = Convert.ToSingle(pointData[1]) - Convert.ToSingle(pointData[3]) + offset + Point_Offset.Y;
                width = Convert.ToSingle(pointData[2]) + offset;
                height = Convert.ToSingle(pointData[3]) + offset;
            }
            else if ("upperright右上".Contains(Properties.Settings.Default.FocusPoint))
            {
                pointAx = Convert.ToSingle(pointData[0]) - Convert.ToSingle(pointData[2]) + offset + Point_Offset.X;
                pointAy = Convert.ToSingle(pointData[1]) + offset + Point_Offset.Y;
                width = Convert.ToSingle(pointData[2]) + offset;
                height = Convert.ToSingle(pointData[3]) + offset;
            }
            else if ("bottomright右下".Contains(Properties.Settings.Default.FocusPoint))
            {
                pointAx = Convert.ToSingle(pointData[0]) - Convert.ToSingle(pointData[2]) + offset + Point_Offset.X;
                pointAy = Convert.ToSingle(pointData[1]) - Convert.ToSingle(pointData[3]) + offset + Point_Offset.Y;
                width = Convert.ToSingle(pointData[2]) + offset;
                height = Convert.ToSingle(pointData[3]) + offset;
            }
            else
            {
                codeOutput("アンカー マッチング失敗.");
                tobeRead.SpeakAsync("アンカー マッチング失敗.");
                return;
            }
            #endregion

            #region 図形を書く
            if (dashFlag)
            {
                picPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                float[] dashArray =
                {
                    5f,     //线长5个像素
                    3.5f,     //间断3.5个像素
                    5f,    //线长15个像素
                    3.5f      //间断3.5个像素
                };
                picPen.DashPattern = dashArray;
                graphObj.DrawRectangle(picPen, pointAx, pointAy, width, height);
            }
            
            if (fillFlag)
            {
                SolidBrush picBru = new SolidBrush(Color.Black);
                graphObj.FillRectangle(picBru, pointAx, pointAy, width, height);
            }

            else
            {
                graphObj.DrawRectangle(picPen, pointAx, pointAy, width, height);
            }
            #endregion

        }
    }
}
