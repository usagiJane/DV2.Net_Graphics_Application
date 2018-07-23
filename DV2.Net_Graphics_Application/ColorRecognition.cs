using System;
using System.Collections.Generic;
using System.Text;

#region Personal Addition
using OpenCvSharp;
#endregion

#region OpenCVSharp online document
//https://shimat.github.io/opencvsharp_docs/html/d75eb659-6335-53f6-af7a-81814a21ab7f.htm
#endregion

namespace DV2.Net_Graphics_Application
{
    class ColorRecognition
    {
        //指先のマーカーを検出する
        public void FindColor(ref Mat srcImg, ref Mat dstImg)
        {
            //Define
            Mat srcImg2HSV, hueImg;
            Vec3b HchImgdata, srcImgdata, pix;
            Mat[] channelsplit;

            //Initialization
            srcImg2HSV = new Mat(srcImg.Size(), srcImg.Type());
            hueImg = new Mat(srcImg.Size(), MatType.CV_8UC1);

            //こうすると、H(彩度)は0～180の範囲の値になる
            Cv2.CvtColor(srcImg, srcImg2HSV, ColorConversionCodes.BGR2HSV);
            //Camera Test window
            //ImShowはMat対象のみ
            //Cv2.ImShow("srcImg2HSV", srcImg2HSV);

            Cv2.Split(srcImg2HSV, out channelsplit);
            hueImg = channelsplit[0];
            pix = srcImg2HSV.At<Vec3b>(0, 0);
            pix[0] = 255;
            pix[1] = 255;
            pix[2] = 255;

            for (int i = 0; i < hueImg.Height; i++)
            {
                for (int j = 0; j < hueImg.Width; j++)
                {
                    //Scalar HchImgdata = Cv2.Get2D(hueImg2Ipl, i, j);
                    HchImgdata = hueImg.At<Vec3b>(i, j);
                    //Scalar srcImgdata = Cv.Get2D(srcImg2HSV2Ipl, i, j);
                    srcImgdata = srcImg2HSV.At<Vec3b>(i, j);
                    //Hueの範囲，赤いから黄色までは0-60，黄色から緑までは60-120，緑から青いまでは120-180，
                    if ((45 <= HchImgdata.Item0 && HchImgdata.Item0 <= 90)) // || (175 <= data.Val0 && data.Val0 <= 180))
                    {
                        //RGBが違う値であるか
                        if (((srcImgdata[0] != srcImgdata[1]) && (srcImgdata[0] != srcImgdata[2])) && (srcImgdata[1] != srcImgdata[2]))
                        {
                            if ((srcImgdata[0] + srcImgdata[2] < 1.2 * (srcImgdata[1])) && (srcImgdata[1] > 30))
                            {
                                //Cv.RGB(255, 255, 255)->白色
                                //Cv.Set2D(dstImg2Ipl, i, j, Cv.RGB(255, 255, 255));
                                dstImg.Set<Vec3b>(i, j, pix);
                            }
                        }
                    }
                }
            }
            srcImg2HSV.Release();
            hueImg.Release();
        }
    }
}
