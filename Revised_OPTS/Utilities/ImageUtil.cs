using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace Inventory_System.Utilities
{
    internal class ImageUtil
    {
        public const int MAX_IMAGE_WIDTH = 1000;
        public const int JPG_QUALITY = 50;

        /// <summary>
        /// This is to reduce the size of the attached picture.
        /// </summary>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        public static byte[] resizeJpg(byte[] sourceData)
        {
            using (Image sourceImage = imageFromByteArray(sourceData))
            {
                int newWidth = sourceImage.Width;
                int newHeight = sourceImage.Height;
                if (newWidth > MAX_IMAGE_WIDTH)
                {
                    newWidth = MAX_IMAGE_WIDTH;
                    newHeight = sourceImage.Height * MAX_IMAGE_WIDTH / sourceImage.Width;
                }
                using (var result = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                    {
                        g.DrawImage(sourceImage, 0, 0, newWidth, newHeight);
                    }

                    ImageCodecInfo ici = ImageCodecInfo.GetImageEncoders().First(v => v.FormatID == ImageFormat.Jpeg.Guid);
                    EncoderParameters eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, JPG_QUALITY);

                    using (var stream = new MemoryStream())
                    {
                        result.Save(stream, ici, eps);
                        return stream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Conversion of the picture to byte array.
        /// </summary>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        public static Image imageFromByteArray(byte[] sourceData)
        {
            using (var ms = new MemoryStream(sourceData))
            {
                return (Bitmap)((new ImageConverter()).ConvertFrom(sourceData));
            }
        }

        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        public static Image RotateImage(Image img)
        {
            var bmp = new Bitmap(img);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.White);
                gfx.DrawImage(img, 0, 0, img.Width, img.Height);
            }

            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }
    }
}
