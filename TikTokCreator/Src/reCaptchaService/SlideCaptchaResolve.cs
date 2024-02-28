using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

namespace reCaptchaService
{
    public class SlideCaptchaResolve : ISlideCaptchaResolve
    {
        public Point Resolve(string param, string NameOrIndex, Bitmap deviceScreen)
        {
            CheckImageFolderExist();
            string Index = NameOrIndex.Substring(NameOrIndex.IndexOf(":") + 1);
            //convert to gray color
             deviceScreen.Save(@"img\captcha-" + Index + ".png");
            var main = deviceScreen.ToImage<Bgr, byte>();
            var img = deviceScreen.ToImage<Gray, byte>();

            img.Save(@"img\gray" + Index + ".png");

            var blur = new Image<Gray, byte>(img.Width, img.Height);
            CvInvoke.GaussianBlur(img, blur, new System.Drawing.Size(3, 3), 0);
            blur.Save(@"img\blur" + Index + ".png");
            var canny = new Image<Gray, byte>(img.Width, img.Height);
            CvInvoke.Canny(blur, canny, 700, 400);
            canny.Save(@"img\canny" + Index + ".png");
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hier = new Mat();
            // tim tat ca cac duong cong trong anh canny luu vao mang
            CvInvoke.FindContours(canny, contours, hier, Emgu.CV.CvEnum.RetrType.List, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            // duyet qua tung duong cong
            for (int i = 0; i < contours.Size; i++)
            {
                var contour = contours[i];
                VectorOfPoint approx = new VectorOfPoint();
                // tinh chu vi duong cong khep kin
                double perimeter = CvInvoke.ArcLength(contour, true);
                CvInvoke.ApproxPolyDP(contours[i], approx, 0.02 * perimeter, true);

                //chi kiem tra nhung hinh co so canh tu 4 den 12 va chu vi lon hon tu 200 den 300
                if (approx.Size >= 4 && approx.Size <= 12 && perimeter > 200 && perimeter < 300)
                {
                    // to mau cho cac vien
                    CvInvoke.DrawContours(main, contours, i, new MCvScalar(255, 0, 255));
                    var momment = CvInvoke.Moments(contour);
                    int cX = (int)Math.Round(momment.M10 / momment.M00);
                    int cY = (int)Math.Round(momment.M01 / momment.M00);
                    // gioi han toa do chi tim kiem trong vung nho
                    if (cX > 150 && cX < 450 && cY > 350 && cY < 590)
                    {
                        CvInvoke.PutText(main, /*Math.Round(perimeter).ToString()*/".", new Point(cX, cY), Emgu.CV.CvEnum.FontFace.HersheyTriplex, 1.0, new MCvScalar(0, 0, 255), 2);
                        main.Save(@"img\find" + Index + ".png");
                        return new Point(cX, cY);
                    }
                }
            }

            return new Point();
        }
        private void CheckImageFolderExist()
        {
            string path = Directory.GetCurrentDirectory() + @"\img";

            try
            {
                if (Directory.Exists(path))
                {
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                throw;

            }
        }
    }
}