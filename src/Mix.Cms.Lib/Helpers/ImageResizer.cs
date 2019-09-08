using Mix.Cms.Lib.Services;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

public class ImageResizer
{

    public static Image cropImage(Image img, Rectangle cropArea)
    {
        Bitmap bmpImage = new Bitmap(img);
        Bitmap bmpCrop = bmpImage.Clone(cropArea,
        bmpImage.PixelFormat);
        return (Image)(bmpCrop);
    }
    public static void ResizeStream(int imageSize, Stream fileStream, string outputPath)
    {
        try
        {
            var image = Image.FromStream(fileStream);
            int thumbnailSize = imageSize;
            int newWidth, newHeight;

            if (image.Width > image.Height)
            {
                newWidth = thumbnailSize;
                newHeight = image.Height * thumbnailSize / image.Width;
            }
            else
            {
                newWidth = image.Width * thumbnailSize / image.Height;
                newHeight = thumbnailSize;
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    thumbnailBitmap.Save(memory, image.RawFormat);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            //thumbnailBitmap.Save(outputPath, image.RawFormat);
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();
        }
        catch (Exception ex)
        {
            string err = ex.Message;
        }
        finally
        {

        }
    }
    public static bool ResizeImage(int imageSize, string base64, string outputPath)
    {
        try
        {
            var img = Base64ToImage(base64);
            ResizeStream(img, outputPath);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static void ResizeStream(Image image, string outputPath)
    {
        try
        {
            int imageSize = MixService.GetConfig<int>("ImageSize");
            int thumbnailSize = imageSize;
            int newWidth, newHeight;

            if (image.Width > image.Height)
            {
                newWidth = thumbnailSize;
                newHeight = image.Height * thumbnailSize / image.Width;
            }
            else
            {
                newWidth = image.Width * thumbnailSize / image.Height;
                newHeight = thumbnailSize;
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    thumbnailBitmap.Save(memory, image.RawFormat);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            //thumbnailBitmap.Save(outputPath, image.RawFormat);
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();
        }
        catch (Exception ex)
        {
            string err = ex.Message;
        }
        finally
        {

        }
    }
    public static Image getResizedImage(String path, float width, float height, bool isCrop)
    {
        return getResizedImage(path, width, height, isCrop, false);
    }

    public static Image getResizedImage(String path, float width, float height, bool isCrop, bool isVertical)
    {
        var image = (Image)new Bitmap(path);

        int thumbnailSize = (int)width;
        int newWidth, newHeight;


        if (image.Width > image.Height)
        {
            newWidth = thumbnailSize;
            newHeight = image.Height * thumbnailSize / image.Width;

            if (isVertical)
            {
                if (newHeight < height)
                {
                    newHeight = thumbnailSize;
                    newWidth = image.Width * thumbnailSize / image.Height;
                }
            }

        }
        else
        {
            newWidth = thumbnailSize;
            newHeight = (image.Height * thumbnailSize) / image.Width; ;
        }

        System.IO.MemoryStream outStream = new System.IO.MemoryStream();

        var thumbnailBitmap = new Bitmap(newWidth, newHeight);

        var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
        thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
        thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
        thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        thumbnailGraph.CompositingMode = CompositingMode.SourceCopy;
        var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);

        thumbnailGraph.DrawImage(image, imageRectangle);
        thumbnailBitmap.Save(outStream, getImageFormat(path));

        byte[] buffer = outStream.ToArray();
        MemoryStream ms = new MemoryStream(buffer);
        Image returnImage = Image.FromStream(ms);

        if (isCrop)
        {


            Bitmap bmp = returnImage as Bitmap;
            height = height > bmp.Height ? bmp.Height : height;
            width = width > bmp.Width ? bmp.Width : width;
            Rectangle cropRect = new Rectangle(0, 0, (int)width, (int)height);
            // Check if it is a bitmap:
            if (bmp == null)
                throw new ArgumentException("No valid bitmap");

            // Crop the image:
            Bitmap cropBmp = bmp.Clone(cropRect, bmp.PixelFormat);

            // Release the resources:
            returnImage.Dispose();

            return cropBmp;
        }

        return returnImage;
    }

    public static string getContentType(String path)
    {
        switch (Path.GetExtension(path))
        {
            case ".bmp": return "Image/bmp";
            case ".gif": return "Image/gif";
            case ".jpg": return "Image/jpeg";
            case ".png": return "Image/png";
            default: break;
        }
        return "";
    }

    public static ImageFormat getImageFormat(String path)
    {
        switch (Path.GetExtension(path))
        {
            case ".bmp": return ImageFormat.Bmp;
            case ".gif": return ImageFormat.Gif;
            case ".jpg": return ImageFormat.Jpeg;
            case ".png": return ImageFormat.Png;
            default: break;
        }
        return ImageFormat.Jpeg;
    }
    public static Image Base64ToImage(string base64String)
    {
        // Convert base 64 string to byte[]
        byte[] imageBytes = Convert.FromBase64String(base64String);
        // Convert byte[] to Image
        using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        {
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}




