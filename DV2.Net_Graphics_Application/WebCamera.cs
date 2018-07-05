using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.CPlusPlus;
using System.Linq;
using System.Text.RegularExpressions;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        internal void CameraStart()
        {
            //カメラのパラメタ
            bool capFlag = true;
            Mat tempImg, flipImg;
            Mat grayImg, renderImg;
            Mat srcImgbyCam = new Mat();
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
                        double a = maxblob.Centroid.X;
                        double b = maxblob.Centroid.Y;
                        //Debug
                        //textBox_X.Text = a.ToString();
                        //textBox_Y.Text = b.ToString();
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
        }

        public System.Drawing.Point FingerFinder(string targetName)
        {
            //To find the finger point which on the Graphics
            //カメラのパラメタ
            bool capFlag = true;
            Mat tempImg, flipImg;
            Mat grayImg, renderImg;
            Mat srcImgbyCam = new Mat();
            double centerX, centerY;
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
            return new System.Drawing.Point(0, 0);
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

            fingerPoint = FingerFinder(targetName);
            PointOnObject(ref fingerPoint, targetName);

            index = ObjectFinder(pointName);
            ObjCommand[index] = "point(" + fingerPoint.X + "," + fingerPoint.Y + ")";
        }

        public void PointOnObject(ref System.Drawing.Point fingerPoint, string targetName)
        {
            //Define
            string targetComm;
            double temp_X = 0, temp_Y = 0;
            List<int> points = new List<int>();
            List<double> avgPointY = new List<double>();
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

            switch (targetComm)
            {
                case "Line":
                    GetLinePoints(targetName, ref points);
                    break;
                case "Circle":
                    GetCirclePoints(targetName, ref points);
                    break;
                //To be continued
                default:
                    codeOutput("Error @WebCamera.cs PointOnObject関数");
                    break;
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
                    codeOutput("Error @WebCamera.cs PointOnObject関数 236行.");
                    tobeRead.SpeakAsync("指先座標点をえることが失敗した,Get命令をもう一度入力して下さい！");
                }
                else
                {
                    double temp_avg = 0.0;
                    foreach(double loop in avgPointY)
                    {
                        temp_avg += loop;
                    }
                    temp_avg = Convert.ToDouble(temp_avg / avgPointY.Count);
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
    }
}
