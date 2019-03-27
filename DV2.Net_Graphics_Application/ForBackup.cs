using System;

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
            int i = 5;
            Console.WriteLine("i is an int? {0}", i.GetType() == typeof(int));
            Console.WriteLine("i is an int? {0}", typeof(int).IsInstanceOfType(i));
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

        #region FindColor
        /*
        public void FindColor(ref Mat srcImg, ref Mat dstImg)
        {
            Mat srcImg2HSV, hueImg;
            IplImage srcImg2Ipl, dstImg2Ipl, srcImg2HSV2Ipl, hueImg2Ipl;

            srcImg2HSV = new Mat(srcImg.Size(), srcImg.Type());
            hueImg = new Mat(srcImg.Size(), MatType.CV_8UC1);
            srcImg2Ipl = srcImg.ToIplImage();
            dstImg2Ipl = dstImg.ToIplImage();
            hueImg2Ipl = hueImg.ToIplImage();
            srcImg2HSV2Ipl = srcImg2HSV.ToIplImage();

            //こうすると、H(彩度)は0～180の範囲の値になる
            Cv.CvtColor(srcImg2Ipl, srcImg2HSV2Ipl, ColorConversion.BgrToHsv);
            //Camera Test window
            //ImShowはMat対象のみ
            Cv2.ImShow("srcImg2HSV", srcImg2HSV);

            Cv.Split(srcImg2HSV2Ipl, hueImg2Ipl, null, null, null);

            for (int i = 0; i < hueImg2Ipl.Height; i++)
            {
                for (int j = 0; j < hueImg2Ipl.Width; j++)
                {
                    CvScalar HchImgdata = Cv.Get2D(hueImg2Ipl, i, j);
                    CvScalar srcImgdata = Cv.Get2D(srcImg2HSV2Ipl, i, j);
                    ////Hueの範囲，赤いから黄色までは0-60，黄色から緑までは60-120，緑から青いまでは120-180，
                    if ((45 <= HchImgdata.Val0 && HchImgdata.Val0 <= 90)) // || (175 <= data.Val0 && data.Val0 <= 180))
                    {
                        //RGBが違う値であるか
                        if (((srcImgdata[0] != srcImgdata[1]) && (srcImgdata[0] != srcImgdata[2])) && (srcImgdata[1] != srcImgdata[2]))
                        {
                            if ((srcImgdata[0] + srcImgdata[2] < 1.2 * (srcImgdata[1])) && (srcImgdata[1] > 30))
                            {
                                //Cv.RGB(255, 255, 255)->白色
                                Cv.Set2D(dstImg2Ipl, i, j, Cv.RGB(255, 255, 255));
                            }
                        }
                    }
                }
            }
            //関数使用方法説明
            //public static OpenCvSharp.CPlusPlus.Mat CvArrToMat(OpenCvSharp.CvArr arr, [bool copyData = false], [bool allowND = true], [int coiMode = 0])
            dstImg = Cv2.CvArrToMat(dstImg2Ipl, true);
            Cv.ReleaseImage(srcImg2Ipl);
            Cv.ReleaseImage(dstImg2Ipl);
            Cv.ReleaseImage(srcImg2HSV2Ipl);
            Cv.ReleaseImage(hueImg2Ipl);
        }
        */
        #endregion

        #region FingerFinder
        /*
        public System.Drawing.Point FingerFinder(string targetName)
        {
            //To find the finger point which on the Graphics
            //カメラのパラメタ
            bool capFlag = true;
            Mat tempImg, flipImg;
            Mat grayImg, renderImg;
            Mat srcImgbyCam = new Mat();
            double centerX = 0.0, centerY = 0.0;
            ColorRecognition iRo = new ColorRecognition();

            using (var capture = new VideoCapture(CaptureDevice.VFW))
            {
                while (capFlag)
                {
                    //カメラから画像をキャプチャする
                    capFlag = capture.Read(srcImgbyCam);
                    if (!capFlag)
                    { break; }
                    //Camera Test window
                    Cv2.ImShow("srcImgbyCam", srcImgbyCam);
                    flipImg = srcImgbyCam.Clone();
                    flipImg = flipImg.Flip(FlipMode.XY);

                    tempImg = Mat.Zeros(srcImgbyCam.Size(), srcImgbyCam.Type());
                    grayImg = new Mat(srcImgbyCam.Size(), MatType.CV_8UC1);
                    //指検出方法
                    //FindColor(ref flipImg, ref tempImg);
                    iRo.FindColor(ref flipImg, ref tempImg);
                    Cv2.CvtColor(tempImg, grayImg, ColorConversion.BgrToGray);
                    Cv2.Threshold(grayImg, grayImg, 100, 255, ThresholdType.Binary);
                    //Mat -> IplImage
                    IplImage grayImg2Ipl = grayImg.ToIplImage();
                    //ラベリング処理
                    CvBlobs blobs = new CvBlobs(grayImg2Ipl);
                    renderImg = new Mat(srcImgbyCam.Size(), MatType.CV_8UC3);
                    //Mat -> IplImage
                    IplImage renderImg2Ipl = renderImg.ToIplImage();
                    IplImage srcImgbyCam2Ipl = srcImgbyCam.ToIplImage();
                    //ラベリング結果の描画
                    blobs.RenderBlobs(srcImgbyCam2Ipl, renderImg2Ipl);
                    //緑最大面積を返す
                    CvBlob maxblob = blobs.LargestBlob();

                    if (maxblob != null)
                    {
                        centerX = maxblob.Centroid.X;
                        centerY = maxblob.Centroid.Y;
                    }

                    int keyValue = Cv2.WaitKey(100);
                    if (keyValue == 27)
                    {
                        CvWindow.DestroyAllWindows();
                        //IplImage 対象Release
                        Cv.ReleaseImage(grayImg2Ipl);
                        Cv.ReleaseImage(renderImg2Ipl);
                        Cv.ReleaseImage(srcImgbyCam2Ipl);
                        capFlag = false;
                        break;   //ESC キーで閉じる
                    }
                }
            }
            return new System.Drawing.Point(Convert.ToInt32(centerX), Convert.ToInt32(centerY));
        }
        */
        #endregion

        #region CameraStart
        /*-
        internal void CameraStart()
        {
            //カメラのパラメタ
            bool capFlag = true;
            Mat tempImg, flipImg;
            Mat grayImg, renderImg;
            Mat srcImgbyCam = new Mat();
            ColorRecognition iRo = new ColorRecognition();

            var capture = new VideoCapture(CaptureDevice.Any)
            {
                //キャプチャする画像のサイズフレームレートの指定
                FrameHeight = 480,
                FrameWidth = 320,
                //FrameHeight = 640, FrameWidth = 480,
            };

            using (capture)
            {
                while (capFlag)
                {
                    //カメラから画像をキャプチャする
                    capFlag = capture.Read(srcImgbyCam);

                    if (srcImgbyCam.Empty())
                    { break; }

                    //Camera Test window
                    Cv2.ImShow("srcImgbyCam", srcImgbyCam);
                    flipImg = srcImgbyCam.Clone();
                    flipImg = flipImg.Flip(FlipMode.XY);

                    tempImg = Mat.Zeros(srcImgbyCam.Size(), srcImgbyCam.Type());
                    grayImg = new Mat(srcImgbyCam.Size(), MatType.CV_8UC1);
                    //指検出方法
                    //FindColor(ref flipImg, ref tempImg);
                    iRo.FindColor(ref flipImg, ref tempImg);

                    Cv2.CvtColor(tempImg, grayImg, ColorConversionCodes.BGR2GRAY);
                    Cv2.Threshold(grayImg, grayImg, 100, 255, ThresholdTypes.Binary);
                    //ラベリング処理
                    CvBlobs blobs = new CvBlobs(grayImg);
                    renderImg = new Mat(srcImgbyCam.Size(), MatType.CV_8UC3);
                    //ラベリング結果の描画
                    blobs.RenderBlobs(srcImgbyCam, renderImg);
                    //緑最大面積を返す
                    CvBlob maxblob = blobs.LargestBlob();

                    if (maxblob != null)
                    {
                        double a = Math.Round(maxblob.Centroid.X, 2);
                        double b = Math.Round(maxblob.Centroid.Y, 2);

                        //手動のキャリブレーション
                        a = (a - 12) / 12.87;
                        b = (b - 43) / 12.40;

                        //For Debug
                        textBox_CenterCoordinates.Text = a.ToString() + "," + b.ToString();
                    }

                    int keyValue = Cv2.WaitKey(100);
                    if (keyValue == 27)
                    {
                        Cv2.DestroyAllWindows();
                        //対象Release
                        tempImg.Release(); flipImg.Release();
                        grayImg.Release(); renderImg.Release();
                        srcImgbyCam.Release();
                        capFlag = false;
                        break;   //ESC キーで閉じる
                    }
                }
            }
        }
        -*/
        #endregion

        #region kaiten
        /*
        public void kaiten()
        {
            zukei tmp;
            int range_x, range_y;
            int value;

            tmp.kind = inf_zukei[obj_num].kind;
            tmp.x_max = inf_zukei[obj_num].x_max;
            tmp.x_min = inf_zukei[obj_num].x_min;
            tmp.x_mid = inf_zukei[obj_num].x_mid;
            tmp.y_max = inf_zukei[obj_num].y_max;
            tmp.y_min = inf_zukei[obj_num].y_min;
            tmp.y_mid = inf_zukei[obj_num].y_mid;

            range_x = tmp.x_max - tmp.x_min;
            range_y = tmp.y_max - tmp.y_min;
            value = range_x - range_y;

            tmp.sx_center = inf_zukei[obj_num].sx_center;
            tmp.sy_center = inf_zukei[obj_num].sy_center;
            tmp.ssita_max = inf_zukei[obj_num].ssita_max;
            tmp.ssita_min = inf_zukei[obj_num].ssita_min;
            tmp.ssita_third = inf_zukei[obj_num].ssita_third;
            tmp.sr = inf_zukei[obj_num].sr;
            tmp.straight_kind = inf_zukei[obj_num].straight_kind;
            tmp.sdegree = inf_zukei[obj_num].sdegree;
            tmp.arrow_kind = inf_zukei[obj_num].arrow_kind;
            tmp.sita_last = inf_zukei[obj_num].sita_last;
            tmp.parameta_niji = inf_zukei[obj_num].parameta_niji;
            tmp.fx_max = inf_zukei[obj_num].fx_max;
            tmp.fx_min = inf_zukei[obj_num].fx_min;
            tmp.fx_mid = inf_zukei[obj_num].fx_mid;
            tmp.fy_max = inf_zukei[obj_num].fy_max;
            tmp.fy_min = inf_zukei[obj_num].fy_min;
            tmp.fy_mid = inf_zukei[obj_num].fy_mid;
            tmp.comment = inf_zukei[obj_num].comment;

            switch (tmp.kind)
            {
                case 0:// 四角形
                    tmp.ssita_third += (Math.PI) / 36.0;
                    if (tmp.ssita_third >= (Math.PI / 2.0))
                        tmp.ssita_third = 0.0;

                    tmp.x_max = 0;
                    tmp.x_min = 95;
                    tmp.y_max = 0;
                    tmp.y_min = 63;

                    double x2, x3;
                    double y2, y3;
                    for (int i = tmp.fx_min; i <= tmp.fx_max; i++)
                    {
                        x2 = (i - tmp.fx_mid) * Math.Cos(tmp.ssita_third) - (tmp.fy_min - tmp.fy_mid) * Math.Sin(tmp.ssita_third) + tmp.fx_mid;
                        y2 = (i - tmp.fx_mid) * Math.Sin(tmp.ssita_third) + (tmp.fy_min - tmp.fy_mid) * Math.Cos(tmp.ssita_third) + tmp.fy_mid;
                        x3 = (i - tmp.fx_mid) * Math.Cos(tmp.ssita_third) - (tmp.fy_max - tmp.fy_mid) * Math.Sin(tmp.ssita_third) + tmp.fx_mid;
                        y3 = (i - tmp.fx_mid) * Math.Sin(tmp.ssita_third) + (tmp.fy_max - tmp.fy_mid) * Math.Cos(tmp.ssita_third) + tmp.fy_mid;
                        if (tmp.x_max < (int)x2)
                            tmp.x_max = (int)x2;
                        if (tmp.x_max < (int)x3)
                            tmp.x_max = (int)x3;
                        if (tmp.x_min > (int)x2)
                            tmp.x_min = (int)x2;
                        if (tmp.x_min > (int)x3)
                            tmp.x_min = (int)x3;

                        if (tmp.y_max < (int)y2)
                            tmp.y_max = (int)y2;
                        if (tmp.y_max < (int)y3)
                            tmp.y_max = (int)y3;
                        if (tmp.y_min > (int)y2)
                            tmp.y_min = (int)y2;
                        if (tmp.y_min > (int)y3)
                            tmp.y_min = (int)y3;
                    }
                    for (int j = tmp.fy_min + 1; j < tmp.fy_max; j++)
                    {
                        x2 = (tmp.fx_min - tmp.fx_mid) * Math.Cos(tmp.ssita_third) - (j - tmp.fy_mid) * Math.Sin(tmp.ssita_third) + tmp.fx_mid;
                        y2 = (tmp.fx_min - tmp.fx_mid) * Math.Sin(tmp.ssita_third) + (j - tmp.fy_mid) * Math.Cos(tmp.ssita_third) + tmp.fy_mid;
                        x3 = (tmp.fx_max - tmp.fx_mid) * Math.Cos(tmp.ssita_third) - (j - tmp.fy_mid) * Math.Sin(tmp.ssita_third) + tmp.fx_mid;
                        y3 = (tmp.fx_max - tmp.fx_mid) * Math.Sin(tmp.ssita_third) + (j - tmp.fy_mid) * Math.Cos(tmp.ssita_third) + tmp.fy_mid;

                        if (tmp.x_max < (int)x2)
                            tmp.x_max = (int)x2;
                        if (tmp.x_max < (int)x3)
                            tmp.x_max = (int)x3;
                        if (tmp.x_min > (int)x2)
                            tmp.x_min = (int)x2;
                        if (tmp.x_min > (int)x3)
                            tmp.x_min = (int)x3;

                        if (tmp.y_max < (int)y2)
                            tmp.y_max = (int)y2;
                        if (tmp.y_max < (int)y3)
                            tmp.y_max = (int)y3;
                        if (tmp.y_min > (int)y2)
                            tmp.y_min = (int)y2;
                        if (tmp.y_min > (int)y3)
                            tmp.y_min = (int)y3;
                    }
                    if (tmp.x_max <= 95 && tmp.y_max <= 63 && tmp.x_min >= 1 && tmp.y_min >= 1)
                    {
                        seikei_w(ref seikei_all_DotData, obj_num, 0);
                        inf_zukei[obj_num] = tmp;
                        seikei_w(ref seikei_all_DotData, obj_num, 2);
                    }
                    break;
                case 1:// ひし形
                    break;
                case 8:// 直線
                    tmp.ssita_third += (Math.PI) / 36.0;
                    //if (tmp.ssita_third == (Math.PI / 2.0))
                    //    tmp.ssita_third = (Math.PI / 2.0) * 3.0 + (Math.PI) / 36.0;
                    if (tmp.ssita_third >= (Math.PI / 2.0) && tmp.ssita_third < (Math.PI * 3.0 / 2.0))
                        tmp.ssita_third = (Math.PI * (3.0 / 2.0));
                    if (tmp.ssita_third >= (Math.PI * 2.0))
                        tmp.ssita_third = 0.0;
                    if (tmp.ssita_third >= ((Math.PI / 2.0) - (Math.PI) / 36.0) && tmp.ssita_third < (Math.PI / 2.0))
                        tmp.ssita_third = (Math.PI / 2.0);

                    int s_x_max = 0;
                    int s_x_min = 95;
                    int s_y_max = 0;
                    int s_y_min = 63;
                    for (double i = 0.0; i <= tmp.sr * 2.0; i += 1.0)
                    {
                        x2 = (tmp.x_mid - tmp.sr - tmp.x_mid + i) * Math.Cos(tmp.ssita_third) - (tmp.y_mid - tmp.y_mid) * Math.Sin(tmp.ssita_third) + tmp.x_mid;
                        y2 = (tmp.x_mid - tmp.sr - tmp.x_mid + i) * Math.Sin(tmp.ssita_third) + (tmp.y_mid - tmp.y_mid) * Math.Cos(tmp.ssita_third) + tmp.y_mid;
                        x2 = Math.Round(x2, 5);
                        y2 = Math.Round(y2, 5);
                        if (s_x_max <= x2)
                            s_x_max = (int)x2;
                        if (s_x_min >= x2)
                            s_x_min = (int)x2;
                        if (s_y_max <= (int)y2)
                            s_y_max = (int)y2;
                        if (s_y_min >= (int)y2)
                            s_y_min = (int)y2;
                    }
                    tmp.x_max = s_x_max;
                    tmp.x_min = s_x_min;
                    tmp.y_max = s_y_max;
                    tmp.y_min = s_y_min;
                    if (tmp.x_max <= 95 && tmp.x_min >= 1 && tmp.y_max <= 63 && tmp.y_min >= 1)
                    {
                        seikei_w(ref seikei_all_DotData, obj_num, 0);
                        inf_zukei[obj_num] = tmp;
                        seikei_w(ref seikei_all_DotData, obj_num, 2);
                    }
                    break;
                case 15://2次関数
                    tmp.parameta_niji = -tmp.parameta_niji;
                    if (tmp.y_min >= 0 && tmp.y_max <= 63)
                    {
                        seikei_w(ref seikei_all_DotData, obj_num, 0);
                        inf_zukei[obj_num] = tmp;
                        seikei_w(ref seikei_all_DotData, obj_num, 2);
                    }
                    break;
                case 12://直線矢印
                    tmp.ssita_third += (Math.PI) / 36.0;
                    if (tmp.ssita_third > ((Math.PI / 2.0) - (Math.PI) / 36.0) && tmp.ssita_third < Math.PI / 2.0)
                        tmp.ssita_third = Math.PI / 2.0;
                    if (tmp.ssita_third > Math.PI - (Math.PI) / 36.0 && tmp.ssita_third < Math.PI)
                        tmp.ssita_third = Math.PI;
                    if (tmp.ssita_third > (Math.PI * 3.0 / 2.0) - (Math.PI) / 36.0 && tmp.ssita_third < Math.PI * 3.0 / 2.0)
                        tmp.ssita_third = Math.PI * 3.0 / 2.0;
                    if (tmp.ssita_third >= Math.PI * 2.0)
                        tmp.ssita_third = 0.0;

                    s_x_max = 0;
                    s_x_min = 95;
                    s_y_max = 0;
                    s_y_min = 63;
                    for (double i = 0.0; i <= tmp.sr * 2.0; i += 1.0)
                    {
                        x2 = (tmp.x_mid - tmp.sr - tmp.x_mid + i) * Math.Cos(tmp.ssita_third) - (tmp.y_mid - tmp.y_mid) * Math.Sin(tmp.ssita_third) + tmp.x_mid;
                        y2 = (tmp.x_mid - tmp.sr - tmp.x_mid + i) * Math.Sin(tmp.ssita_third) + (tmp.y_mid - tmp.y_mid) * Math.Cos(tmp.ssita_third) + tmp.y_mid;
                        x2 = Math.Round(x2, 5);
                        y2 = Math.Round(y2, 5);
                        if (s_x_max <= x2)
                            s_x_max = (int)x2;
                        if (s_x_min >= x2)
                            s_x_min = (int)x2;
                        if (s_y_max <= (int)y2)
                            s_y_max = (int)y2;
                        if (s_y_min >= (int)y2)
                            s_y_min = (int)y2;
                    }
                    tmp.x_max = s_x_max;
                    tmp.x_min = s_x_min;
                    tmp.y_max = s_y_max;
                    tmp.y_min = s_y_min;
                    if (tmp.x_max <= 95 && tmp.x_min >= 1 && tmp.y_max <= 63 && tmp.y_min >= 1)
                    {
                        seikei_w(ref seikei_all_DotData, obj_num, 0);
                        inf_zukei[obj_num] = tmp;
                        seikei_w(ref seikei_all_DotData, obj_num, 2);
                    }
                    break;
                case 16://長方形

                    tmp.ssita_third += (Math.PI) / 36.0;
                    if (tmp.ssita_third >= Math.PI)
                        tmp.ssita_third = 0.0;

                    tmp.x_max = 0;
                    tmp.x_min = 95;
                    tmp.y_max = 0;
                    tmp.y_min = 63;

                    for (int i = tmp.fx_min; i <= tmp.fx_max; i++)
                    {
                        x2 = (i - tmp.fx_mid) * Math.Cos(tmp.ssita_third) - (tmp.fy_min - tmp.fy_mid) * Math.Sin(tmp.ssita_third) + tmp.fx_mid;
                        y2 = (i - tmp.fx_mid) * Math.Sin(tmp.ssita_third) + (tmp.fy_min - tmp.fy_mid) * Math.Cos(tmp.ssita_third) + tmp.fy_mid;
                        x3 = (i - tmp.fx_mid) * Math.Cos(tmp.ssita_third) - (tmp.fy_max - tmp.fy_mid) * Math.Sin(tmp.ssita_third) + tmp.fx_mid;
                        y3 = (i - tmp.fx_mid) * Math.Sin(tmp.ssita_third) + (tmp.fy_max - tmp.fy_mid) * Math.Cos(tmp.ssita_third) + tmp.fy_mid;
                        if (tmp.x_max < (int)x2)
                            tmp.x_max = (int)x2;
                        if (tmp.x_max < (int)x3)
                            tmp.x_max = (int)x3;
                        if (tmp.x_min > (int)x2)
                            tmp.x_min = (int)x2;
                        if (tmp.x_min > (int)x3)
                            tmp.x_min = (int)x3;

                        if (tmp.y_max < (int)y2)
                            tmp.y_max = (int)y2;
                        if (tmp.y_max < (int)y3)
                            tmp.y_max = (int)y3;
                        if (tmp.y_min > (int)y2)
                            tmp.y_min = (int)y2;
                        if (tmp.y_min > (int)y3)
                            tmp.y_min = (int)y3;
                    }
                    for (int j = tmp.fy_min + 1; j < tmp.fy_max; j++)
                    {
                        x2 = (tmp.fx_min - tmp.fx_mid) * Math.Cos(tmp.ssita_third) - (j - tmp.fy_mid) * Math.Sin(tmp.ssita_third) + tmp.fx_mid;
                        y2 = (tmp.fx_min - tmp.fx_mid) * Math.Sin(tmp.ssita_third) + (j - tmp.fy_mid) * Math.Cos(tmp.ssita_third) + tmp.fy_mid;
                        x3 = (tmp.fx_max - tmp.fx_mid) * Math.Cos(tmp.ssita_third) - (j - tmp.fy_mid) * Math.Sin(tmp.ssita_third) + tmp.fx_mid;
                        y3 = (tmp.fx_max - tmp.fx_mid) * Math.Sin(tmp.ssita_third) + (j - tmp.fy_mid) * Math.Cos(tmp.ssita_third) + tmp.fy_mid;

                        if (tmp.x_max < (int)x2)
                            tmp.x_max = (int)x2;
                        if (tmp.x_max < (int)x3)
                            tmp.x_max = (int)x3;
                        if (tmp.x_min > (int)x2)
                            tmp.x_min = (int)x2;
                        if (tmp.x_min > (int)x3)
                            tmp.x_min = (int)x3;

                        if (tmp.y_max < (int)y2)
                            tmp.y_max = (int)y2;
                        if (tmp.y_max < (int)y3)
                            tmp.y_max = (int)y3;
                        if (tmp.y_min > (int)y2)
                            tmp.y_min = (int)y2;
                        if (tmp.y_min > (int)y3)
                            tmp.y_min = (int)y3;
                    }
                    if (tmp.x_max <= 95 && tmp.y_max <= 63 && tmp.x_min >= 1 && tmp.y_min >= 1)
                    {
                        seikei_w(ref seikei_all_DotData, obj_num, 0);
                        inf_zukei[obj_num] = tmp;
                        seikei_w(ref seikei_all_DotData, obj_num, 2);
                    }
                    break;
            }
        }
        */
        #endregion

    }
    //End of class Backup_Programs
}