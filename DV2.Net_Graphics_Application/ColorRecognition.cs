using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
#endregion

namespace DV2.Net_Graphics_Application
{
    class ColorRecognition
    {
        //指先のマーカーを検出する
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
                    //Hueの範囲，赤いから黄色までは0-60，黄色から緑までは60-120，緑から青いまでは120-180，
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
    }
}
