using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.CPlusPlus;
using System.Text.RegularExpressions;
#endregion

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        public void CameraStart()
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

        //Sample
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

        public Point FingerFinder(string targetName)
        {
            //To find the finger point which on the Graphics
            return new Point(0, 0);
        }

        public void GetPointOnObject(string objCommandData, string objAnalysisData)
        {
            //"Get p on obj1",命令の入口
            //objCommandData	"get|p|on|obj1"
            //objAnalysisData	"Get|Ident|On|Ident"
            LogOutput("@WebCamera GetPointOnObject");
            //Define
            Point fingerPoint = new Point(0, 0);
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

        public void PointOnObject(ref Point fingerPoint, string targetName)
        {
            //Define
            double temp_X = 0, temp_Y = 0;
            List<double> points = new List<double>();
            List<double> avgPointY = new List<double>();
            List<Point> LinePoints = new List<Point>();
            

            //Get all point
            //再修正
            GetLinePoints(targetName, ref points);

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
                    
                }
                else
                {

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
