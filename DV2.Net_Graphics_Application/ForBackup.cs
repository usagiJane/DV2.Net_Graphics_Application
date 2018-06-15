using System;
using System.Collections.Generic;
using System.Text;

namespace DV2.Net_Graphics_Application
{
    /// <summary>
    /// All for BackUp the Programs, Just in Case.
    /// </summary>
    public partial class Backup_Programs
    {
        #region LogOutput
        private void LogOutput(Object log)
        {
            //Nothing!
        }
        #endregion
        #region LogOutput 
        /*
        //MainForm mfObj = null;
        //bool PointerVerify_flag = false;
        private void PointerVerify(MainForm pv)
        {
            //Verify The MainForm Pointer
            if (mfObj == null && mfObj != pv)
            {
                mfObj = pv;
                PointerVerify_flag = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error @DV2_Drawing.cs PointerVerify Function!" + "\r\n" + "There are something wrong with the MainForm Pointer!" + "\r\n" + "Please CHECK the Function!", "Notice @DV2_Drawing.cs", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                System.Environment.Exit(0);
            }
        }
        private void LogOutput(Object log)
        {
            if (PointerVerify_flag)
            {
                mfObj.textBox_log.AppendText(log + "\r\n");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error @DV2_Drawing.cs LogOutput Function!" + "\r\n" + "We Lost the MainForm Pointer!" + "\r\n" + "Please CHECK the Function!", "Notice @DV2_Drawing.cs", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                System.Environment.Exit(0);
            }
        }
        */
        #endregion

        /* DrawImgButton_Clicked
        private void DrawImgButton_Clicked(object sender, EventArgs e)
        {
            float offset = 35f;
            Size picSize = picBox.Size;
            Bitmap image = new Bitmap(picSize.Width, picSize.Height);
            Pen picPen = new Pen(Color.LightBlue, 2.7F);
            Graphics graphObj = Graphics.FromImage(image);

            LogOutput("picSize.Width: " + picSize.Width);
            LogOutput("picSize.Height: " + picSize.Height);
            LogOutput("comboBox Pictype SelectedIndex: " + comboBox_codeType.SelectedIndex);
            LogOutput("System String Processing Strated!");

            graphObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphObj.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphObj.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //Draw Data
            //graphImg.DrawString("GraphicsData", new Font("Century", 10), new SolidBrush(Color.Blue), new PointF(picSize.Width - 50, picSize.Height - 50));
            //Draw Line
            if (comboBox_codeType.SelectedIndex == 0 && textBox_code.TextLength != 0)
            {
                ////例 line(1, 1, 100, 100)
                LogOutput(DateTime.Now.ToString("HH:mm") + " textBox_code入力データ: " + textBox_code.Text);
                //define
                string[] txtBoxdata, txtBoxSubdata;

                txtBoxdata = textBox_code.Text.Replace("\r\n", "").Replace('(', ',').Replace(")", "").Split(';');

                //平行判断
                if (textBox_code.Text.Contains("||"))
                {
                    LogOutput(DateTime.Now.ToString("HH:mm") + " 平行判断の流れ ");
                    LogOutput(DateTime.Now.ToString("HH:mm") + " textboxdata: " + textBox_code.Text);
                }



                LogOutput(DateTime.Now.ToString("HH:mm") + " txtBoxdata.Length: " + txtBoxdata.Length);
                //Loop textBoxdata
                for (int i = 0; i < txtBoxdata.Length - 1; i++)
                {
                    txtBoxSubdata = txtBoxdata[i].Split(',');
                    if (txtBoxSubdata[0].ToLower() == "line" && txtBoxSubdata.Length == 5)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " 画像ラインを描く");
                        float pointAx = Convert.ToSingle(txtBoxSubdata[1]) + offset;
                        float pointAy = Convert.ToSingle(txtBoxSubdata[2]) + offset;
                        float pointBx = Convert.ToSingle(txtBoxSubdata[3]) + offset;
                        float pointBy = Convert.ToSingle(txtBoxSubdata[4]) + offset;

                        graphObj.DrawLine(picPen, pointAx, pointAy, pointBx, pointBy);
                    }
                    else if (txtBoxSubdata[0].ToLower() == "arc" && txtBoxSubdata.Length == 7)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " 画像曲線を描く");
                        float pointAx = Convert.ToSingle(txtBoxSubdata[1]) + offset;
                        float pointAy = Convert.ToSingle(txtBoxSubdata[2]) + offset;
                        float width = Convert.ToSingle(txtBoxSubdata[3]) + offset;
                        float height = Convert.ToSingle(txtBoxSubdata[4]) + offset;
                        float startAngle = Convert.ToSingle(txtBoxSubdata[5]) + offset;
                        float sweepAngle = Convert.ToSingle(txtBoxSubdata[6]) + offset;

                        //方法一
                        //Rectangle rect = new Rectangle(Convert.ToInt16(pointAx), Convert.ToInt16(pointAy), Convert.ToInt16(width), Convert.ToInt16(height));
                        ///DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle);
                        //graphObj.DrawArc(picPen, rect, 45.0F, 135.0F);
                        //方法二
                        ///DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle);
                        graphObj.DrawArc(picPen, pointAx, pointAy, width, height, startAngle, sweepAngle);
                    }
                    else if (txtBoxSubdata[0].ToLower() == "circle" && txtBoxSubdata.Length == 5)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " 画像円を描く");
                        float pointAx = Convert.ToSingle(txtBoxSubdata[1]) + offset;
                        float pointAy = Convert.ToSingle(txtBoxSubdata[2]) + offset;
                        float width = Convert.ToSingle(txtBoxSubdata[3]) + offset;
                        float height = Convert.ToSingle(txtBoxSubdata[4]) + offset;

                        graphObj.DrawEllipse(picPen, pointAx, pointAy, width, height);
                    }
                    else if (txtBoxSubdata[0].ToLower() == "arrow" && txtBoxSubdata.Length == 5)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " 画像矢印を描く");
                        float pointAx = Convert.ToSingle(txtBoxSubdata[1]) + offset;
                        float pointAy = Convert.ToSingle(txtBoxSubdata[2]) + offset;
                        float pointBx = Convert.ToSingle(txtBoxSubdata[3]) + offset;
                        float pointBy = Convert.ToSingle(txtBoxSubdata[4]) + offset;

                        picPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        graphObj.DrawLine(picPen, pointAx, pointAy, pointBx, pointBy);
                        picPen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                    }
                    else if (txtBoxSubdata[0].ToLower() == "square" && txtBoxSubdata.Length == 5)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " 画像正方形を描く");
                        float pointAx = Convert.ToSingle(txtBoxSubdata[1]) + offset;
                        float pointAy = Convert.ToSingle(txtBoxSubdata[2]) + offset;
                        float width = Convert.ToSingle(txtBoxSubdata[3]) + offset;
                        float height = Convert.ToSingle(txtBoxSubdata[4]) + offset;
                        //check data
                        ///DrawRectangle(Pen pen, float x, float y, float width, float height);
                        graphObj.DrawRectangle(picPen, pointAx, pointAy, width, height);
                    }
                    else if (txtBoxSubdata[0].ToLower() == "rectangle" && txtBoxSubdata.Length == 5)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " 画像長方形を描く");
                        float pointAx = Convert.ToSingle(txtBoxSubdata[1]) + offset;
                        float pointAy = Convert.ToSingle(txtBoxSubdata[2]) + offset;
                        float width = Convert.ToSingle(txtBoxSubdata[3]) + offset;
                        float height = Convert.ToSingle(txtBoxSubdata[4]) + offset;
                        //check data
                        ///DrawRectangle(Pen pen, float x, float y, float width, float height);
                        graphObj.DrawRectangle(picPen, pointAx, pointAy, width, height);
                    }
                    else if (txtBoxSubdata[0].ToLower() == "rhombus" && txtBoxSubdata.Length == 5)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " 画像菱形を描く");
                    }
                    else if (txtBoxSubdata[0].ToLower() == "sin" && txtBoxSubdata.Length == 1)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " Sin形を描く");
                    }
                    else if (txtBoxSubdata[0].ToLower() == "cos" && txtBoxSubdata.Length == 1)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " Cos形を描く");
                    }
                    else if (txtBoxSubdata[0].ToLower() == "triangle" && txtBoxSubdata.Length == 7)
                    {
                        LogOutput(DateTime.Now.ToString("HH:mm") + " 三角形を描く");
                        float pointAx = Convert.ToSingle(txtBoxSubdata[1]) + offset;
                        float pointAy = Convert.ToSingle(txtBoxSubdata[2]) + offset;
                        float pointBx = Convert.ToSingle(txtBoxSubdata[3]) + offset;
                        float pointBy = Convert.ToSingle(txtBoxSubdata[4]) + offset;
                        float pointCx = Convert.ToSingle(txtBoxSubdata[5]) + offset;
                        float pointCy = Convert.ToSingle(txtBoxSubdata[6]) + offset;
                        Point pointA = new Point(Convert.ToInt32(pointAx), Convert.ToInt32(pointAy));
                        Point pointB = new Point(Convert.ToInt32(pointBx), Convert.ToInt32(pointBy));
                        Point pointC = new Point(Convert.ToInt32(pointCx), Convert.ToInt32(pointCy));
                        Point[] triangle = { pointA, pointB, pointC };

                        graphObj.DrawPolygon(picPen, triangle);
                    }
                }
            }
            //座標軸を描く
            drawAxis(ref image, ref graphObj);
            //座標軸の
            drawAxisXYpart(ref image, ref graphObj, 0f, 300f, 0f, 300f, 10);

            #region imgShow
            tabControl1.SelectTab(1);
            //image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            picBox.Image = (Image)image;
            #endregion

            #region Save Image Data
            string filename = @"D:\AllTestDatas\";
            filename += "DV2.Net_Graphics_";
            filename += DateTime.Now.ToString().Replace(":", "").Replace("/", "");
            filename = filename.Replace(" ", "_");
            filename += ".Jpg";
            image.Save(filename);
            #endregion
        }
        */

        #region Keys.Enter Original backup from EnterKeyPress
        /*
        string txtBoxdata;
        string[] storageData, txtBoxSubdata;
        if (e.KeyCode == Keys.Enter && false)
        {
            LogOutput("Enter Test");
            LogOutput(DateTime.Now.ToString("HH:mm") + " textBox_Input 入力データ: " + textBox_Input.Text);

            if (textBox_Input.Text.Contains(":="))
            {
                //storageData = textBox_Input.Text.Split(':=');　同じ意味
                storageData = Regex.Split(textBox_Input.Text, ":=", RegexOptions.IgnoreCase);

                ObjName.Add(storageData[0]);
                ObjCommand.Add(storageData[1]);
            }

            //改行コードと括弧とセミコロンと空白(スペース)を削除する
            txtBoxdata = textBox_Input.Text.Replace("\r\n", "").Replace('(', ',').Replace(")", "").Replace(";", "").Replace(" ", "");
            LogOutput(DateTime.Now.ToString("HH:mm") + " txtBoxdata データ: " + txtBoxdata);

            txtBoxSubdata = txtBoxdata.Split(',');

            if (txtBoxSubdata[0].Substring(0, 3).ToLower() == "obj")
            //Obj対象処理
            {
                GraphicDraw.AutoSelectmode();
            }

            else if (txtBoxSubdata[0].ToLower() == "plot" || txtBoxSubdata[0].ToLower() == "show")
            //対象を表示する
            {

            }

            else if (txtBoxSubdata[0].ToLower() == "solve")
            //数式パラメーター計算
            {
                //FormulaAnalysis();
            }
            else
            {
                LogOutput("命令認識できない");
                tobeRead.Speak("命令認識できない");
            }
        }
        */
        #endregion

        #region Graphic_line
        /*
        private void drawLine(ref ArrayList commData, Pen picPen = null, float offset = pub_offSet)
        {
            //graphObj 
            //picPen
            picPen = picPen ?? pub_picPen;
            //offset 変位量
            //commData 命令文内容,可変パラメータ
            float pointAx = 0, pointAy = 0, pointBx = 0, pointBy = 0;

            if (commData.Count == 5)
            {
                //座標計算
                pointAx = Convert.ToSingle(commData[1]) + offset;
                pointAy = Convert.ToSingle(commData[2]) + offset;
                pointBx = Convert.ToSingle(commData[3]) + offset;
                pointBy = Convert.ToSingle(commData[4]) + offset;
            }


            graphObj.DrawLine(picPen, pointAx, pointAy, pointBx, pointBy);
        }

        public void drawArrow()
        {
            //LogOutput("矢印");
            //int pointX1 = 0, pointY1 = 0, pointX2 = 0, pointY2 = 0;
            //string point1, point2;
            //string[] point11, point22;

            //point1 = textBox2.Text;
            //point2 = textBox3.Text;
            //point11 = point1.Split(',');
            //point22 = point2.Split(',');
            //int.TryParse(point11[0], out pointX1);
            //int.TryParse(point11[1], out pointY1);
            //int.TryParse(point22[0], out pointX2);
            //int.TryParse(point22[1], out pointY2);
            ////picPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            //picPen.EndCap = System.Drawing.Drawing2D.LineCap.Triangle;

            ////矢印を書く
            //graphObj.DrawLine(picPen, pointX1, pointY1, pointX2, pointY2);
        }
        */
        #endregion
    }
}