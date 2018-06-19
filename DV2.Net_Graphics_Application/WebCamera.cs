using System;
using System.Collections.Generic;
using System.Text;
#region Append
using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.CPlusPlus;
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

        public void FingerFinder()
        {

        }
    }
}
