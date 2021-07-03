using Mix.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mix.Shared.Services
{
    public class MixFileService: SingletonService<MixFileService>
    {
        public string CurrentDirectory { get; set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="MixFileRepository"/> class from being created.
        /// </summary>
        public MixFileService()
        {
            CurrentDirectory = Environment.CurrentDirectory;
        }

        #region Read Files

        public FileViewModel GetFile(
            string name,
            string ext,
            string FileFolder,
            bool isCreate = false,
            string defaultContent = null)
        {
            FileViewModel result = null;

            string fullPath = $"{CurrentDirectory}/{FileFolder}/{name}{ext}";

            FileInfo fileinfo = new FileInfo(fullPath);

            if (fileinfo.Exists)
            {
                try
                {
                    using (var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader s = new StreamReader(stream))
                        {
                            result = new FileViewModel()
                            {
                                FileFolder = FileFolder,
                                Filename = name,
                                Extension = ext,
                                Content = s.ReadToEnd()
                            };
                        }
                    }
                }
                catch
                {
                    // File invalid
                }
            }
            else if (isCreate)
            {
                CreateDirectoryIfNotExist(FileFolder);
                fileinfo.Create();
                result = new FileViewModel()
                {
                    FileFolder = FileFolder,
                    Filename = name,
                    Extension = ext,
                    Content = defaultContent
                };
                SaveFile(result);
            }

            return result ?? new FileViewModel() { FileFolder = FileFolder };
        }
        #endregion

        #region Create / Delete File or Folder
        public void CreateDirectoryIfNotExist(string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        public bool CopyDirectory(string srcPath, string desPath)
        {
            if (srcPath.ToLower() != desPath.ToLower() && Directory.Exists(srcPath))
            {
                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(srcPath, desPath));
                }

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(srcPath, desPath), true);
                }

                return true;
            }
            return true;
        }

        public bool SaveFile(FileViewModel file)
        {
            try
            {
                if (!string.IsNullOrEmpty(file.Filename))
                {
                    CreateDirectoryIfNotExist(file.FileFolder);

                    string filePath = $"{file.Filename}{file.Extension}";
                    if (!string.IsNullOrEmpty(file.FileFolder))
                    {
                        filePath = $"{file.FileFolder}/{filePath}";
                    }
                    if (File.Exists(filePath))
                    {
                        DeleteFile(filePath);
                    }
                    if (!string.IsNullOrEmpty(file.Content))
                    {
                        using (var writer = File.CreateText(filePath))
                        {
                            writer.WriteLine(file.Content); //or .Write(), if you wish
                            writer.Dispose();
                            return true;
                        }
                    }
                    else if (file.FileStream != null)
                    {
                        string base64 = file.FileStream.Split(',')[1];
                        byte[] bytes = Convert.FromBase64String(base64);
                        using (var writer = File.Create(filePath))
                        {
                            writer.Write(bytes, 0, bytes.Length);
                            return true;
                        }
                    }
                    else
                    {
                        File.CreateText(filePath);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                bool result = false;
                File.Delete(filePath);
                result = true;
                return result;
            }
            return true;
        }

        #endregion
    }
}
