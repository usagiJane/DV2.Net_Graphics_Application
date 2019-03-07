using System;

#region Personal Addition
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;
using System.Drawing.Imaging;
#endregion

namespace DV2.Net_Graphics_Application
{
    //class Graphic_Rotation
    public partial class MainForm
    {
        #region MarkFlags
        private bool BeRotated = false;
        #endregion

        /// <summary>
        /// 画像を個別回転用関数，現在未使用
        /// </summary>
        public void Graphic_Rotation()
        {
            #region enko
            /*
            //enko_kaiten
            int enko_kind = 0;//右回転
            enko_kind = 1;//左回転
            if (enko_kind == 0)
            {
                tmp.ssita_max += 0.17444;
                tmp.ssita_min += 0.17444;
                tmp.ssita_third += 0.17444;
                tmp.sita_last += 0.17444;

                if (tmp.ssita_max > ((Math.PI) * 2))
                    tmp.ssita_max = tmp.ssita_max - ((Math.PI) * 2);
                if (tmp.ssita_min > ((Math.PI) * 2))
                    tmp.ssita_min = tmp.ssita_min - ((Math.PI) * 2);
                if (tmp.ssita_third > ((Math.PI) * 2))
                    tmp.ssita_third = tmp.ssita_third - ((Math.PI) * 2);
                if (tmp.sita_last > ((Math.PI) * 2))
                    tmp.sita_last = tmp.sita_last - ((Math.PI) * 2);
            }
            else
            {
                tmp.ssita_max -= 0.17444;
                tmp.ssita_min -= 0.17444;
                tmp.ssita_third -= 0.17444;
                tmp.sita_last -= 0.17444;

                if (tmp.ssita_max < 0.0)
                    tmp.ssita_max = tmp.ssita_max + ((Math.PI) * 2);
                if (tmp.ssita_min < 0.0)
                    tmp.ssita_min = tmp.ssita_min + ((Math.PI) * 2);
                if (tmp.ssita_third < 0.0)
                    tmp.ssita_third = tmp.ssita_third + ((Math.PI) * 2);
                if (tmp.sita_last < 0.0)
                    tmp.sita_last = tmp.sita_last + ((Math.PI) * 2);
            }
            if (tmp.ssita_max < tmp.ssita_min)
            {
                double save_sita = tmp.ssita_max;
                tmp.ssita_max = tmp.ssita_min;
                tmp.ssita_min = save_sita;
            }
            if ((tmp.ssita_third <= tmp.ssita_max) && (tmp.ssita_min <= tmp.ssita_third))//範囲がπ以上なら別処理
            {
                for (double i = tmp.ssita_min; i <= tmp.ssita_max; i += 0.05)
                {
                    arc_point_y = Math.Sin(i);
                    arc_point_x = Math.Cos(i);
                    if (max_x <= (tmp.sx_center + (tmp.sr * arc_point_x)))
                        max_x = (int)(tmp.sx_center + (tmp.sr * arc_point_x));
                    if (min_x >= (tmp.sx_center + (tmp.sr * arc_point_x)))
                        min_x = (int)(tmp.sx_center + (tmp.sr * arc_point_x));
                    if (max_y <= (tmp.sy_center + (tmp.sr * arc_point_y)))
                        max_y = (int)(tmp.sy_center + (tmp.sr * arc_point_y));
                    if (min_y >= (tmp.sy_center + (tmp.sr * arc_point_y)))
                        min_y = (int)(tmp.sy_center + (tmp.sr * arc_point_y));
                }
            }
            else
            {
                for (double i = 0.0; i <= tmp.ssita_min; i += 0.05)
                {
                    arc_point_y = Math.Sin(i);
                    arc_point_x = Math.Cos(i);
                    if (max_x <= (tmp.sx_center + (tmp.sr * arc_point_x)))
                        max_x = (int)(tmp.sx_center + (tmp.sr * arc_point_x));
                    if (min_x >= (tmp.sx_center + (tmp.sr * arc_point_x)))
                        min_x = (int)(tmp.sx_center + (tmp.sr * arc_point_x));
                    if (max_y <= (tmp.sy_center + (tmp.sr * arc_point_y)))
                        max_y = (int)(tmp.sy_center + (tmp.sr * arc_point_y));
                    if (min_y >= (tmp.sy_center + (tmp.sr * arc_point_y)))
                        min_y = (int)(tmp.sy_center + (tmp.sr * arc_point_y));
                }
                for (double i = tmp.ssita_max; i <= (Math.PI) * 2; i += 0.05)
                {
                    arc_point_y = Math.Sin(i);
                    arc_point_x = Math.Cos(i);
                    if (max_x <= (tmp.sx_center + (tmp.sr * arc_point_x)))
                        max_x = (int)(tmp.sx_center + (tmp.sr * arc_point_x));
                    if (min_x >= (tmp.sx_center + (tmp.sr * arc_point_x)))
                        min_x = (int)(tmp.sx_center + (tmp.sr * arc_point_x));
                    if (max_y <= (tmp.sy_center + (tmp.sr * arc_point_y)))
                        max_y = (int)(tmp.sy_center + (tmp.sr * arc_point_y));
                    if (min_y >= (tmp.sy_center + (tmp.sr * arc_point_y)))
                        min_y = (int)(tmp.sy_center + (tmp.sr * arc_point_y));
                }
            }
            tmp.x_max = max_x;
            tmp.x_min = min_x;
            tmp.y_max = max_y;
            tmp.y_min = min_y;

            if (tmp.x_max <= 95 && tmp.x_min >= 1 && tmp.y_max <= 63 && tmp.y_min >= 1)
            {
                seikei_w(ref seikei_all_DotData, obj_num, 0);
                inf_zukei[obj_num] = tmp;
                seikei_w(ref seikei_all_DotData, obj_num, 2);
            }
            */
            #endregion enko

            #region line
            //line kaiten
            /*
            switch (line_mode)
            {
                case 0://直線
                    syn.SpeakAsync("回転");
                    if (tmp.ssita_third >= (Math.PI * 3.0 / 2.0) && tmp.ssita_third <= (Math.PI * 2.0))
                    {
                        tmp.ssita_third = 0.0;
                        syn.SpeakAsync("0度");
                    }
                    else if (tmp.ssita_third >= 0 && tmp.ssita_third <= (Math.PI * 2.0))
                    {
                        tmp.ssita_third = (Math.PI * 3.0 / 2.0);
                        syn.SpeakAsync("90度");
                    }

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
                case 1://直線矢印右回転
                    syn.SpeakAsync("右回転");
                    if (tmp.ssita_third >= (Math.PI * 3.0 / 2.0) && tmp.ssita_third <= (Math.PI * 2.0))
                    {
                        tmp.ssita_third = 0.0;
                        syn.SpeakAsync("0度");
                    }
                    else if (tmp.ssita_third >= 0 && tmp.ssita_third < (Math.PI / 2.0))
                    {
                        tmp.ssita_third = (Math.PI / 2.0);
                        syn.SpeakAsync("90度");
                    }
                    else if (tmp.ssita_third >= (Math.PI / 2.0) && tmp.ssita_third < (Math.PI))
                    {
                        tmp.ssita_third = (Math.PI);
                        syn.SpeakAsync("180度");
                    }
                    else if (tmp.ssita_third >= (Math.PI) && tmp.ssita_third < (Math.PI * 3.0 / 2.0))
                    {
                        tmp.ssita_third = (Math.PI * 3.0 / 2.0);
                        syn.SpeakAsync("270度");
                    }

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
                case 2://直線矢印左回転
                    syn.SpeakAsync("左回転");
                    if (tmp.ssita_third > 0.0 && tmp.ssita_third <= (Math.PI / 2.0))
                    {
                        tmp.ssita_third = 0.0;
                        syn.SpeakAsync("0度");
                    }
                    else if (tmp.ssita_third > (Math.PI * 3.0 / 2.0) && tmp.ssita_third <= (Math.PI * 2.0) || tmp.ssita_third == 0.0)
                    {
                        tmp.ssita_third = (Math.PI * 3.0 / 2.0);
                        syn.SpeakAsync("270度");
                    }
                    else if (tmp.ssita_third > (Math.PI) && tmp.ssita_third <= (Math.PI * 3.0 / 2.0))
                    {
                        tmp.ssita_third = (Math.PI);
                        syn.SpeakAsync("180度");
                    }
                    else if (tmp.ssita_third > (Math.PI / 2.0) && tmp.ssita_third <= (Math.PI))
                    {
                        tmp.ssita_third = (Math.PI / 2.0);
                        syn.SpeakAsync("90度");
                    }

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
            }
            */
            #endregion


        }

        /// <summary>
        /// 画像回転用関数，
        /// </summary>
        /// <param name="ObjComm"></param>
        /// <param name="ObjAna"></param>
        private void Rotation(string ObjComm, string ObjAna)
        {
            //bool fk;
            double degree = 45;
            Bitmap outputImage;
            LogOutput("for Debug");
            //fk = Rotation(debug_Image, degree,out outputImage);
            //outputImage = Rotation(debug_Image, (int) degree);
            outputImage = rotateImage(debug_Image, (int)degree);


            picBox.Image = (Image)outputImage;
        }

        /// <summary>
        /// 
        /// </summary>
        public void oldkaiten()
        {
            /*
            zukei tmp;
            int range_x, range_y;
            int value;
            int obj_num;

            tmp.kind = inf_zukei[obj_num].kind;//図形種類
            tmp.x_max = inf_zukei[obj_num].x_max;//max_x = 31 + width(0);
            tmp.x_min = inf_zukei[obj_num].x_min;//min_x = 16 + width(0);
            tmp.x_mid = inf_zukei[obj_num].x_mid;//mid_x = (max_x - min_x) / 2 + min_x;
            tmp.y_max = inf_zukei[obj_num].y_max;//max_y = 22 + height;
            tmp.y_min = inf_zukei[obj_num].y_min;//min_y = 8 + height;
            tmp.y_mid = inf_zukei[obj_num].y_mid;//mid_y = (max_y - min_y) / 2 + min_y;

            range_x = tmp.x_max - tmp.x_min;
            range_y = tmp.y_max - tmp.y_min;
            value = range_x - range_y;

            tmp.sx_center = inf_zukei[obj_num].sx_center;//center_x1 = 0;
            tmp.sy_center = inf_zukei[obj_num].sy_center;//center_y1 = 0;
            tmp.ssita_max = inf_zukei[obj_num].ssita_max;//sita_max1 = 0.0;
            tmp.ssita_min = inf_zukei[obj_num].ssita_min;//sita_min1 = 0.0;
            tmp.ssita_third = inf_zukei[obj_num].ssita_third;//sita_third1 = 0.0;
            tmp.sr = inf_zukei[obj_num].sr;//r1 = 0.0;
            tmp.straight_kind = inf_zukei[obj_num].straight_kind;//straight_kind1 = 0;
            tmp.sita_last = inf_zukei[obj_num].sita_last;//sita_last1 = 0.0;
            tmp.parameta_niji = inf_zukei[obj_num].parameta_niji;//parameta_niji1 = 0.0;
            tmp.fx_max = inf_zukei[obj_num].fx_max;//fx_max1 = 31 + width;
            tmp.fx_min = inf_zukei[obj_num].fx_min;//fx_min1 = 16 + width;
            tmp.fx_mid = inf_zukei[obj_num].fx_mid;//fx_mid1 = (max_x + min_x) / 2 ;
            tmp.fy_max = inf_zukei[obj_num].fy_max;//fy_max1 = 22 + height;
            tmp.fy_min = inf_zukei[obj_num].fy_min;//fy_min1 = 8 + height;
            tmp.fy_mid = inf_zukei[obj_num].fy_mid;//fy_mid1 = (max_y + min_y) / 2 ;
            tmp.comment = inf_zukei[obj_num].comment;//comment1 = "";

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

                        for (double i = 0.0; i <= r2 * 2.0; i += 1.0)
                        {
                            x2 = (mid_x - (int)r2 - mid_x + i) * Math.Cos(sita_third) - (mid_y - mid_y) * Math.Sin(sita_third) + mid_x;
                            y2 = (mid_x - (int)r2 - mid_x + i) * Math.Sin(sita_third) + (mid_y - mid_y) * Math.Cos(sita_third) + mid_y;
                            x2 = Math.Round(x2, 5);
                            y2 = Math.Round(y2, 5);
                            DotData[(int)x2, (int)y2] = value;

                        }
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
            */
        }

        public void Rotation(double degree)
        {
            //Define
            double x2, x3;
            double y2, y3;

            //tmp.ssita_third += (Math.PI) / 36.0;
            if (degree >= (Math.PI / 2.0) && degree < (Math.PI * 3.0 / 2.0))
                degree = (Math.PI * (3.0 / 2.0));
            if (degree >= (Math.PI * 2.0))
                degree = 0.0;
            if (degree >= ((Math.PI / 2.0) - (Math.PI) / 36.0) && degree < (Math.PI / 2.0))
                degree = (Math.PI / 2.0);

            int s_x_max = 0, x_max, x_min, y_max, y_min, x_mid=0, y_mid=0,r=0;
            int s_x_min = 95;
            int s_y_max = 0;
            int s_y_min = 63;
            for (double i = 0.0; i <= r * 2.0; i += 1.0)
            {
                x2 = (x_mid - r - x_mid + i) * Math.Cos(degree) - (y_mid - y_mid) * Math.Sin(degree) + x_mid;
                y2 = (x_mid - r - x_mid + i) * Math.Sin(degree) + (y_mid - y_mid) * Math.Cos(degree) + y_mid;
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
            x_max = s_x_max;
            x_min = s_x_min;
            y_max = s_y_max;
            y_min = s_y_min;
            if (x_max <= 95 && x_min >= 1 && y_max <= 63 && y_min >= 1)
            {
                //seikei_w(ref seikei_all_DotData, obj_num, 0);
                //inf_zukei[obj_num] = tmp;
                //seikei_w(ref seikei_all_DotData, obj_num, 2);
            }
        }

        public Bitmap Rotation(Bitmap b, int angle)
        {
            angle = angle % 360;
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);
            //重至绘图的所有变换
            g.ResetTransform();
            g.Save();
            g.Dispose();
            //dsImage.Save("yuancd.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return dsImage;
        }

        public Bitmap rotateImage(Bitmap b, int angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(returnBitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //move rotation point to center of image
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            //draw passed in image onto graphics object
            g.DrawImage(b, new Point(0, 0));
            return returnBitmap;
        }

        /// <summary>
        /// Rotation
        /// </summary>
        /// <param name="srcBmp">input</param>
        /// <param name="degree">回転角度</param>
        /// <param name="dstBmp">output</param>
        /// <returns> true / false </returns>
        public bool Rotation(Bitmap srcBmp, double degree, out Bitmap dstBmp)
        {
            if (srcBmp == null)
            {
                dstBmp = null;
                return false;
            }
            dstBmp = null;
            BitmapData srcBmpData = null;
            BitmapData dstBmpData = null;
            switch ((int)degree)
            {
                case 0:
                    dstBmp = new Bitmap(srcBmp);
                    break;
                case -90:
                    dstBmp = new Bitmap(srcBmp.Height, srcBmp.Width);
                    srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    unsafe
                    {
                        byte* ptrSrc = (byte*)srcBmpData.Scan0;
                        byte* ptrDst = (byte*)dstBmpData.Scan0;
                        for (int i = 0; i < srcBmp.Height; i++)
                        {
                            for (int j = 0; j < srcBmp.Width; j++)
                            {
                                ptrDst[j * dstBmpData.Stride + (dstBmp.Height - i - 1) * 3] = ptrSrc[i * srcBmpData.Stride + j * 3];
                                ptrDst[j * dstBmpData.Stride + (dstBmp.Height - i - 1) * 3 + 1] = ptrSrc[i * srcBmpData.Stride + j * 3 + 1];
                                ptrDst[j * dstBmpData.Stride + (dstBmp.Height - i - 1) * 3 + 2] = ptrSrc[i * srcBmpData.Stride + j * 3 + 2];
                            }
                        }
                    }
                    srcBmp.UnlockBits(srcBmpData);
                    dstBmp.UnlockBits(dstBmpData);
                    break;
                case 90:
                    dstBmp = new Bitmap(srcBmp.Height, srcBmp.Width);
                    srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    unsafe
                    {
                        byte* ptrSrc = (byte*)srcBmpData.Scan0;
                        byte* ptrDst = (byte*)dstBmpData.Scan0;
                        for (int i = 0; i < srcBmp.Height; i++)
                        {
                            for (int j = 0; j < srcBmp.Width; j++)
                            {
                                ptrDst[(srcBmp.Width - j - 1) * dstBmpData.Stride + i * 3] = ptrSrc[i * srcBmpData.Stride + j * 3];
                                ptrDst[(srcBmp.Width - j - 1) * dstBmpData.Stride + i * 3 + 1] = ptrSrc[i * srcBmpData.Stride + j * 3 + 1];
                                ptrDst[(srcBmp.Width - j - 1) * dstBmpData.Stride + i * 3 + 2] = ptrSrc[i * srcBmpData.Stride + j * 3 + 2];
                            }
                        }
                    }
                    srcBmp.UnlockBits(srcBmpData);
                    dstBmp.UnlockBits(dstBmpData);
                    break;
                case 180:
                case -180:
                    dstBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
                    srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    unsafe
                    {
                        byte* ptrSrc = (byte*)srcBmpData.Scan0;
                        byte* ptrDst = (byte*)dstBmpData.Scan0;
                        for (int i = 0; i < srcBmp.Height; i++)
                        {
                            for (int j = 0; j < srcBmp.Width; j++)
                            {
                                ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3] = ptrSrc[i * srcBmpData.Stride + j * 3];
                                ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3 + 1] = ptrSrc[i * srcBmpData.Stride + j * 3 + 1];
                                ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3 + 2] = ptrSrc[i * srcBmpData.Stride + j * 3 + 2];
                            }
                        }
                    }
                    srcBmp.UnlockBits(srcBmpData);
                    dstBmp.UnlockBits(dstBmpData);
                    break;
                default://任意角度
                    double radian = degree * Math.PI / 180.0;//将角度转换为弧度
                                                             //计算正弦和余弦
                    double sin = Math.Sin(radian);
                    double cos = Math.Cos(radian);
                    //计算旋转后的图像大小
                    int widthDst = (int)(srcBmp.Height * Math.Abs(sin) + srcBmp.Width * Math.Abs(cos));
                    int heightDst = (int)(srcBmp.Width * Math.Abs(sin) + srcBmp.Height * Math.Abs(cos));

                    dstBmp = new Bitmap(widthDst, heightDst);
                    //确定旋转点
                    int dx = (int)(srcBmp.Width / 2 * (1 - cos) + srcBmp.Height / 2 * sin);
                    int dy = (int)(srcBmp.Width / 2 * (0 - sin) + srcBmp.Height / 2 * (1 - cos));

                    int insertBeginX = srcBmp.Width / 2 - widthDst / 2;
                    int insertBeginY = srcBmp.Height / 2 - heightDst / 2;

                    //插值公式所需参数
                    double ku = insertBeginX * cos - insertBeginY * sin + dx;
                    double kv = insertBeginX * sin + insertBeginY * cos + dy;
                    double cu1 = cos, cu2 = sin;
                    double cv1 = sin, cv2 = cos;

                    double fu, fv, a, b, F1, F2;
                    int Iu, Iv;
                    srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    unsafe
                    {
                        byte* ptrSrc = (byte*)srcBmpData.Scan0;
                        byte* ptrDst = (byte*)dstBmpData.Scan0;
                        for (int i = 0; i < heightDst; i++)
                        {
                            for (int j = 0; j < widthDst; j++)
                            {
                                fu = j * cu1 - i * cu2 + ku;
                                fv = j * cv1 + i * cv2 + kv;
                                if ((fv < 1) || (fv > srcBmp.Height - 1) || (fu < 1) || (fu > srcBmp.Width - 1))
                                {

                                    ptrDst[i * dstBmpData.Stride + j * 3] = 150;
                                    ptrDst[i * dstBmpData.Stride + j * 3 + 1] = 150;
                                    ptrDst[i * dstBmpData.Stride + j * 3 + 2] = 150;
                                }
                                else
                                {//双线性插值
                                    Iu = (int)fu;
                                    Iv = (int)fv;
                                    a = fu - Iu;
                                    b = fv - Iv;
                                    for (int k = 0; k < 3; k++)
                                    {
                                        F1 = (1 - b) * *(ptrSrc + Iv * srcBmpData.Stride + Iu * 3 + k) + b * *(ptrSrc + (Iv + 1) * srcBmpData.Stride + Iu * 3 + k);
                                        F2 = (1 - b) * *(ptrSrc + Iv * srcBmpData.Stride + (Iu + 1) * 3 + k) + b * *(ptrSrc + (Iv + 1) * srcBmpData.Stride + (Iu + 1) * 3 + k);
                                        *(ptrDst + i * dstBmpData.Stride + j * 3 + k) = (byte)((1 - a) * F1 + a * F2);
                                    }
                                }
                            }
                        }
                    }
                    srcBmp.UnlockBits(srcBmpData);
                    dstBmp.UnlockBits(dstBmpData);
                    break;
            }
            return true;
        }
    }
}