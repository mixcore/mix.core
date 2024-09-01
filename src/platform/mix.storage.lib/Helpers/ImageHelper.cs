using Mix.Identity.ViewModels;
using Mix.Storage.Lib.Models;
using Org.BouncyCastle.Utilities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Storage.Lib.Helpers
{
    public class ImageHelper
    {
        public static bool IsImageResizeable(string extension)
        {
            string[] exts = new string[] { ".img", ".png", ".jpg", ".jpeg" };
            return exts.Contains(extension.ToLower());
        }

        #region Async Methods

        public static async Task<bool> SaveImageAsync(Stream fileStream, FileModel file, ImageSize? size = null)
        {
            if (!string.IsNullOrEmpty(file.FileBase64))
            {
                return await SaveBase64Async(file, size);

            }
            else
            {
                return await SaveFileStreamAsync(fileStream, file, size);
            }
        }

        private static async Task<bool> SaveFileStreamAsync(Stream fileStream, FileModel file, ImageSize? size)
        {
            using (Image image = await Image.LoadAsync(fileStream))
            {
                if (size != null)
                {
                    int width = size.Width;
                    int height = (image.Height * width) / image.Width;
                    image.Mutate(x => x.Resize(width, height));
                    string fullPath = file.FullPath.Replace(file.Extension, $"-{size.Name}{file.Extension}");
                    image.Save(fullPath);
                }
                else
                {
                    await image.SaveAsync(file.FullPath);
                }
                return true;
            }
        }

        private static async Task<bool> SaveBase64Async(FileModel file, ImageSize? size)
        {
            string base64 = file.FileBase64.IndexOf(',') >= 0
                       ? file.FileBase64.Split(',')[1]
                       : file.FileBase64;
            byte[] bytes = Convert.FromBase64String(base64);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (Image image = await Image.LoadAsync(stream))
                {
                    if (size != null)
                    {
                        int width = size.Width;
                        int height = (image.Height * width) / image.Width;
                        image.Mutate(x => x.Resize(width, height));
                        image.Save(file.FullPath);
                    }
                    else
                    {
                        image.Save(file.FullPath);
                    }
                    return true;
                }
            }
        }
        #endregion

        #region Sync Methods

        public static bool SaveImage(Stream fileStream, FileModel file, ImageSize? size = null)
        {
            if (!string.IsNullOrEmpty(file.FileBase64))
            {
                return SaveBase64(file, size);

            }
            else
            {
                return SaveFileStream(fileStream, file, size);
            }
        }

        private static bool SaveFileStream(Stream fileStream, FileModel file, ImageSize? size)
        {
            var format = Image.DetectFormat(fileStream);
            using (Image image = Image.Load(fileStream))
            {
                if (size != null)
                {
                    int width = size.Width;
                    int height = (image.Height * width) / image.Width;
                    image.Mutate(x => x.Resize(width, height));
                    string fullPath = file.FullPath.Replace(file.Extension, $"-{size.Name}{file.Extension}");
                    image.Save(fullPath);
                }
                else
                {
                    image.Save(file.FullPath);
                }
                return true;
            }
        }

        private static bool SaveBase64(FileModel file, ImageSize? size)
        {
            string base64 = file.FileBase64.IndexOf(',') >= 0
                       ? file.FileBase64.Split(',')[1]
                       : file.FileBase64;
            byte[] bytes = Convert.FromBase64String(base64);
            using (Image image = Image.Load(bytes))
            {
                if (size != null)
                {
                    int width = size.Width;
                    int height = (image.Height * width) / image.Width;
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(file.FullPath);
                }
                else
                {
                    image.Save(file.FullPath);
                }
                return true;
            }
        }
        #endregion
    }
}
