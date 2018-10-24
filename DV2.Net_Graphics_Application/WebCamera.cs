using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using OpenCvSharp;
using OpenCvSharp.Blob;
using System.Linq;
using System.Text.RegularExpressions;
#endregion

#region OpenCVSharp online document
//https://shimat.github.io/opencvsharp_docs/html/d75eb659-6335-53f6-af7a-81814a21ab7f.htm
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        bool capFlag = false;

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
                        textBox2.Text = a.ToString() + "," + b.ToString();
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

        public System.Drawing.Point FingerFinder(string targetName = "")
        {
            //To find the finger point which on the Graphics
            //カメラのパラメタ
            capFlag = true;
            Mat tempImg, flipImg;
            Mat grayImg, renderImg;
            Mat srcImgbyCam = new Mat();
            double centerX = 0.0, centerY = 0.0;
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
                    iRo.FindColor(ref flipImg, ref tempImg);

                    Cv2.CvtColor(tempImg, grayImg, ColorConversionCodes.BGR2GRAY);
                    Cv2.Threshold(grayImg, grayImg, 100, 255, ThresholdTypes.Binary);
                    //ラベリング処理
                    //CvBlobs blobs = new CvBlobs(grayImg2Ipl);
                    CvBlobs blobs = new CvBlobs(grayImg);
                    renderImg = new Mat(srcImgbyCam.Size(), MatType.CV_8UC3);
                    //ラベリング結果の描画
                    blobs.RenderBlobs(srcImgbyCam, renderImg);
                    //緑最大面積を返す
                    CvBlob maxblob = blobs.LargestBlob();

                    if (maxblob != null)
                    {
                        centerX = Math.Round(maxblob.Centroid.X, 2);
                        centerY = Math.Round(maxblob.Centroid.Y, 2);

                        //手動のキャリブレーション
                        centerX = (int)((centerX - 12) / 12.87);
                        centerY = (int)((centerY - 43) / 12.40);

                        //手動のキャリブレーション Ⅱ
                        centerX = (int)((centerX - 2) * 2);
                        centerY = (int)((centerY - 1) * 2);

                        //For Debug
                        textBox2.Text = centerX.ToString() + " , " + centerY.ToString();
                    }

                    int keyValue = Cv2.WaitKey(100);
                    if (keyValue == 27)
                    {
                        Window.DestroyAllWindows();
                        //対象Release
                        tempImg.Release(); flipImg.Release();
                        grayImg.Release(); renderImg.Release();
                        srcImgbyCam.Release();
                        capFlag = false;
                        break;   //ESC キーで閉じる
                    }
                }
            }
            return new System.Drawing.Point(Convert.ToInt32(centerX + movement.X), Convert.ToInt32(centerY + movement.Y));
        }

        public System.Drawing.Point FingerFinder(string targetName, bool dashFlag = false)
        {
            //未完成
            //To find the finger point which on the Graphics
            return new System.Drawing.Point(0, 0);
        }

        public void GetPointOnObject(string objCommandData, string objAnalysisData)
        {
            //"Get p on obj1",命令の入口
            //objCommandData	"get|p|on|obj1"
            //objAnalysisData	"Get|Ident|On|Ident"
            LogOutput("@WebCamera GetPointOnObject");
            //Define
            System.Drawing.Point fingerPoint = new System.Drawing.Point(0, 0);
            int index;
            string targetName = "";
            string pointName = "";
            string[] commList = Regex.Split(objCommandData, @"\|", RegexOptions.IgnoreCase);
            string[] anaList = Regex.Split(objAnalysisData, @"\|", RegexOptions.IgnoreCase);
            List<string> identList = new List<string>();

            //Ident型の変数名を取り出す
            for (int i = 0; i < anaList.Length; i++)
            {
                if (anaList[i] == "Ident")
                {
                    identList.Add(commList[i]);
                }
            }

            //取り出した変数名が代表された命令文を判断する,Lineのみ残す
            for (int m = 0; m < identList.Count; m++)
            {
                for (int n = 0; n < ObjName.Count; n++)
                {
                    if (identList[m] == ObjName[n].ToString())
                    {
                        if (Regex.Split(ObjAnalysis[n].ToString(), @"\|", RegexOptions.IgnoreCase)[0] == "Point")
                        {
                            pointName = identList[m];
                        }
                        if (Regex.Split(ObjAnalysis[n].ToString(), @"\|", RegexOptions.IgnoreCase)[0] == "Line")
                        {
                            targetName = identList[m];
                        }
                    }
                }
            }

            //指先座標Get
            fingerPoint = FingerFinder(targetName);
            PointOnObject(ref fingerPoint, targetName);

            index = ObjectFinder(pointName);
            if (index != -1)
            { 
                ObjCommand[index] = "Point|(|" + fingerPoint.X + "|,|" + fingerPoint.Y + "|)";
                //計算結果を表示する
                textBox3.Text = fingerPoint.X.ToString() + " , " + fingerPoint.Y.ToString();
                codeOutput(">" + fingerPoint.X.ToString() + " , " + fingerPoint.Y.ToString());
                tobeRead.SpeakAsync("座標点は" + fingerPoint.X.ToString() + " , " + fingerPoint.Y.ToString() + "となる.");

                int dataGridView_index = this.dataGridView_monitor.Rows.Add();
                this.dataGridView_monitor.Rows[dataGridView_index].Cells[0].Value = pointName;
                this.dataGridView_monitor.Rows[dataGridView_index].Cells[1].Value = "point("+fingerPoint.X+","+fingerPoint.Y+")";
                this.dataGridView_monitor.Rows[dataGridView_index].Cells[2].Value = "Point|Lparen|IntNum|Comma|IntNum|Rparen";
            }
            else
            {
                codeOutput("対象点" + pointName + "は未定義!");
                tobeRead.SpeakAsync("対象点" + pointName + "は未定義!");
            }
        }

        public void PointOnObject(ref System.Drawing.Point fingerPoint, string targetName)
        {
            //Define
            string targetComm;
            int temp_X = 0, temp_Y = 0;
            List<int> points = new List<int>();
            List<int> avgPointY = new List<int>();
            List<Point> LinePoints = new List<Point>();
            //System.Drawing.Point fingerPoint = new System.Drawing.Point(fPx, fPy);

            //Processing
            //Get all point
            //再修正
            if (ObjectFinder(targetName) < 0)
            {
                //Error
                tobeRead.SpeakAsync(targetName + "が存在しない。 Error @WebCamera.cs PointOnObject関数");
                return;
            }
            else
            { 
                targetComm = ObjCommand[ObjectFinder(targetName)].ToString();
                AssignRemover(ref targetComm);
            }

            targetComm = Regex.Split(targetComm, @"\|", RegexOptions.IgnoreCase)[0];

            switch (targetComm.ToLower())
            {
                case "line":
                    GetLinePoints(targetName, ref points);
                    break;
                case "circle":
                    GetCirclePoints(targetName, ref points);
                    break;
                //To be continued
                default:
                    codeOutput("Error @WebCamera.cs PointOnObject関数 switch部分");
                    return;
            }

            //座標点集合を回す、OpenCVのPoint型Listを作る
            for (int i = 0; i < points.Count; i++)
            {
                if (i % 2 == 0)
                {
                    temp_X = points[i];
                }
                else
                {
                    temp_Y = points[i];
                }
                LinePoints.Add(new Point(temp_X, temp_Y));
            }

            fingerPoint.X = fingerPoint.X - movement.X;
            //マーカーの中心座標に似ている、座標点数を数える、X座標を基準として。
            foreach (Point loop in LinePoints)
            {
                if (loop.X == fingerPoint.X)
                {
                    avgPointY.Add(loop.Y);
                }
            }

            if (avgPointY.Count == 1)
            {
                fingerPoint.Y = Convert.ToInt32(avgPointY[0]);
            }
            else if (avgPointY.Count > 1)
            {
                int temp;
                avgPointY.Sort();
                temp = avgPointY.BinarySearch(fingerPoint.Y);
                if (temp < 0)
                {
                    //Y座標値は直線座標集合の中に存在しない
                    //未完成、処理方法は不明
                    //codeOutput("Error @WebCamera.cs PointOnObject関数 236行.");
                    //tobeRead.SpeakAsync("指先座標点をえることが失敗した,Get命令をもう一度実行して下さい！");

                    int temp_avg = 0;
                    foreach (int loop in avgPointY)
                    {
                        temp_avg += loop;
                    }
                    temp_avg = Convert.ToInt32(temp_avg / avgPointY.Count);
                    temp = avgPointY.BinarySearch(temp_avg);
                    if (temp < 0)
                    {
                        //Find the nearest
                        //Use the LINQ system library to solve the problem.
                        var result = (from x in avgPointY select new { Key = x, Value = Math.Abs(x - temp_avg) }).OrderBy(x => x.Value);
                        //result.ToList().ForEach(x => Console.Write(x.Key + " "));
                        fingerPoint.Y = Convert.ToInt32(result.ToList()[0].Key);
                    }
                    else
                    {
                        fingerPoint.Y = Convert.ToInt32(temp_avg);
                    }
                }
                else
                {
                    int temp_avg = 0;
                    foreach(int loop in avgPointY)
                    {
                        temp_avg += loop;
                    }
                    temp_avg = Convert.ToInt32(temp_avg / avgPointY.Count);
                    temp = avgPointY.BinarySearch(temp_avg);
                    if(temp < 0)
                    {
                        //Find the nearest
                        //Use the LINQ system library to solve the problem.
                        var result = (from x in avgPointY select new { Key = x, Value = Math.Abs(x - temp_avg) }).OrderBy(x => x.Value);
                        //result.ToList().ForEach(x => Console.Write(x.Key + " "));
                        fingerPoint.Y = Convert.ToInt32(result.ToList()[0].Key);
                    }
                    else
                    {
                        fingerPoint.Y = Convert.ToInt32(temp_avg);
                    }
                }
            }
            else
            {
                //Error 識別失敗
                tobeRead.SpeakAsync("Error,@WebCamera PointOnObject関数,指先座標が識別失敗しました.");
                return;
            }
        }

        public int GetPixelData(int hei, int wid)
        {
            
            return -1;
        }
    }
}
