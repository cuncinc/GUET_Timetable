using Tesseract;
using System.IO;
using SkiaSharp;

//namespace GUET.BackEnd.Utils
//{
////    class OcrUtil
//    {
//        //public static string GetCodeFromImg(SKBitmap bitmap)
//        //{
//        //    return ImgToTxt(bitmap);
//        //}

//        public static string GetCodeFromStream(Stream stream)
//        {
//            return ImgToTxt(stream);
//        }

//        private static string ImgToTxt(MemoryStream stream)
//        {
//            //预处理图片
//            var bitmap = SKBitmap.Decode(stream);
//            preDealImg(bitmap);

//            //转换为Pix
//            byte[] bytes = stream.ToArray();
//            var img = Pix.LoadFromMemory(bytes);
//            //使用tesseract获取图片的内容
//            using var engine = new TesseractEngine(@"Utils\tessdata\", "eng", EngineMode.Default);
//            engine.SetVariable("tessedit_char_whitelist", "0123456789");
//            using var page = engine.Process(img);
//            return page.GetText();
//        }

//        private static void preDealImg(SKBitmap bitmap)
//        {
//            //灰度化
//            ToGrey(bitmap);
//            //去除噪点
//            ClearNoise(bitmap);

//            //string path = @"E:\CC\Desktop";
//            //bitmap.Save(path+@"\gray.jpg");
//            ////锐化
//            //Sharpen(bitmap);
//            //bitmap.Save(path+@"\shapen.jpg");
//            //bitmap.Save(path + @"\clrNoise.jpg");
//            ////二值化
//            //Thresholding(bitmap);
//            //bitmap.Save(path+@"\two.jpg");
//        }

//        //灰度化
//        static private void ToGrey(SKBitmap bitmap)
//        {
//            for (int i = 0; i < bitmap.Width; i++)
//            {
//                for (int j = 0; j < bitmap.Height; j++)
//                {
//                    Color pixelColor = bitmap.GetPixel(i, j);
//                    //计算灰度值
//                    int grey = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
//                    Color newColor = Color.FromArgb(grey, grey, grey);
//                    bitmap.SetPixel(i, j, newColor);
//                }
//            }
//        }

//        //二值化
//        static private void Thresholding(SKBitmap bitmap)
//        {
//            int[] histogram = new int[256];
//            int minGrayValue = 255, maxGrayValue = 0;
//            //求取直方图
//            for (int i = 0; i < bitmap.Width; i++)
//            {
//                for (int j = 0; j < bitmap.Height; j++)
//                {
//                    Color pixelColor = bitmap.GetPixel(i, j);
//                    histogram[pixelColor.R]++;
//                    if (pixelColor.R > maxGrayValue) maxGrayValue = pixelColor.R;
//                    if (pixelColor.R < minGrayValue) minGrayValue = pixelColor.R;
//                }
//            }
//            //迭代计算阀值
//            int threshold = -1;
//            int newThreshold = (minGrayValue + maxGrayValue) / 2;
//            for (int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++)
//            {
//                threshold = newThreshold;
//                int lP1 = 0;
//                int lP2 = 0;
//                int lS1 = 0;
//                int lS2 = 0;
//                //求两个区域的灰度的平均值
//                for (int i = minGrayValue; i < threshold; i++)
//                {
//                    lP1 += histogram[i] * i;
//                    lS1 += histogram[i];
//                }
//                int mean1GrayValue = (lP1 / lS1);
//                for (int i = threshold + 1; i < maxGrayValue; i++)
//                {
//                    lP2 += histogram[i] * i;
//                    lS2 += histogram[i];
//                }
//                int mean2GrayValue = (lP2 / lS2);
//                newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
//            }
//            //计算二值化
//            for (int i = 0; i < bitmap.Width; i++)
//            {
//                for (int j = 0; j < bitmap.Height; j++)
//                {
//                    Color pixelColor = bitmap.GetPixel(i, j);
//                    if (pixelColor.R > threshold) bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
//                    else bitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0));
//                }
//            }
//        }

//        //去除噪点
//        static private void ClearNoise(SKBitmap bitmap)
//        {
//            int dgGrayValue = 150;    //背景与前景的灰度界限
//            Color piexl;
//            int MaxNearPoints = 2;
//            // 逐点判断
//            for (int i = 0; i < bitmap.Width; i++)
//                for (int j = 0; j < bitmap.Height; j++)
//                {
//                    piexl = bitmap.GetPixel(i, j);
//                    if (piexl.R < dgGrayValue)
//                    {
//                        int nearDots = 0;
//                        // 判断周围8个点是否全为空
//                        if (i == 0 || i == bitmap.Width - 1 || j == 0 || j == bitmap.Height - 1)   // 边框全去掉
//                        {
//                            bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
//                        }
//                        else
//                        {
//                            if (bitmap.GetPixel(i - 1, j - 1).R < dgGrayValue) nearDots++;
//                            if (bitmap.GetPixel(i, j - 1).R < dgGrayValue) nearDots++;
//                            if (bitmap.GetPixel(i + 1, j - 1).R < dgGrayValue) nearDots++;
//                            if (bitmap.GetPixel(i - 1, j).R < dgGrayValue) nearDots++;
//                            if (bitmap.GetPixel(i + 1, j).R < dgGrayValue) nearDots++;
//                            if (bitmap.GetPixel(i - 1, j + 1).R < dgGrayValue) nearDots++;
//                            if (bitmap.GetPixel(i, j + 1).R < dgGrayValue) nearDots++;
//                            if (bitmap.GetPixel(i + 1, j + 1).R < dgGrayValue) nearDots++;
//                        }

//                        if (nearDots < MaxNearPoints)
//                            bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));    // 去掉单点 && 粗细小3邻边点
//                    }
//                    else    // 背景
//                        bitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
//                }
//        }

//        //锐化，有bug，调用这个方法会闪退
//        static private void Sharpen(SKBitmap bitmap)
//        {
//            Color pixel;
//            Bitmap newBitmap = new Bitmap(bitmap);
//            //拉普拉斯模板
//            int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
//            for (int x = 1; x < bitmap.Width - 1; x++)
//            {
//                for (int y = 1; y < bitmap.Height - 1; y++)
//                {
//                    int r = 0, g = 0, b = 0;
//                    int Index = 0;
//                    for (int col = -1; col <= 1; col++)
//                    {
//                        for (int row = -1; row <= 1; row++)
//                        {
//                            pixel = bitmap.GetPixel(x + row, y + col); r += pixel.R * Laplacian[Index];
//                            g += pixel.G * Laplacian[Index];
//                            b += pixel.B * Laplacian[Index];
//                            Index++;
//                        }
//                    }
//                    //处理颜色值溢出
//                    r = r > 255 ? 255 : r;
//                    r = r < 0 ? 0 : r;
//                    g = g > 255 ? 255 : g;
//                    g = g < 0 ? 0 : g;
//                    b = b > 255 ? 255 : b;
//                    b = b < 0 ? 0 : b;
//                    newBitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
//                    bitmap.Dispose();
//                    bitmap = newBitmap;
//                }
//            }
//        }
//    }
//}
