// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Mix.Cms.Lib.Repositories
{
    public class FileRepository
    {
        public string CurrentDirectory { get; set; }

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile FileRepository instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static FileRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new FileRepository();
                    }
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FileRepository"/> class from being created.
        /// </summary>
        private FileRepository()
        {
            CurrentDirectory = Environment.CurrentDirectory;
        }

        public FileViewModel GetFile(string FilePath, List<FileViewModel> Files, string FileFolder)
        {
            var result = Files.Find(v => !string.IsNullOrEmpty(FilePath) && v.Filename == FilePath.Replace(@"\", "/").Split('/')[1]);
            return result ?? new FileViewModel() { FileFolder = FileFolder };
        }

        public FileViewModel GetWebFile(string filename, string folder)
        {
            string fullPath = CommonHelper.GetFullPath(new string[]
            {
                MixConstants.Folder.WebRootPath,
                folder,
                filename
            });
            string folderPath = CommonHelper.GetFullPath(new string[]
            {
                MixConstants.Folder.WebRootPath,
                MixConstants.Folder.FileFolder,
                folder
            });
            FileInfo file = new FileInfo(fullPath);
            FileViewModel result = null;
            try
            {
                DirectoryInfo path = new DirectoryInfo(folderPath);
                using (StreamReader s = file.OpenText())
                {
                    result = new FileViewModel()
                    {
                        FolderName = path.Name,
                        FileFolder = folder,
                        Filename = file.Name.Substring(0, file.Name.LastIndexOf('.')),
                        Extension = file.Extension,
                        Content = s.ReadToEnd()
                    };
                }
            }
            catch
            {
                // File invalid
            }

            return result ?? new FileViewModel() { FileFolder = folder };
        }

        public bool DeleteWebFile(string filename, string folder)
        {
            string fullPath = CommonHelper.GetFullPath(new string[]
           {
                MixConstants.Folder.WebRootPath,
                MixConstants.Folder.FileFolder,
                folder,
                filename
           });

            if (File.Exists(fullPath))
            {
                CommonHelper.RemoveFile(fullPath);
            }
            return true;
        }

        public bool DeleteWebFile(string filePath)
        {
            string fullPath = CommonHelper.GetFullPath(new string[]
           {
                MixConstants.Folder.WebRootPath,
                filePath
           });

            if (File.Exists(fullPath))
            {
                CommonHelper.RemoveFile(fullPath);
            }
            return true;
        }

        public bool DeleteWebFolder(string folderPath)
        {
            string fullPath = CommonHelper.GetFullPath(new string[]
            {
                MixConstants.Folder.WebRootPath,
                folderPath
            });

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }
            return true;
        }

        public FileViewModel GetUploadFile(string name, string ext, string FileFolder)
        {
            FileViewModel result = null;

            string folder = CommonHelper.GetFullPath(new string[] { MixConstants.Folder.UploadFolder, FileFolder });
            string fullPath = string.Format(@"{0}/{1}.{2}", folder, name, ext);

            FileInfo file = new FileInfo(fullPath);

            try
            {
                using (StreamReader s = file.OpenText())
                {
                    result = new FileViewModel()
                    {
                        FileFolder = FileFolder,
                        Filename = file.Name.Substring(0, file.Name.LastIndexOf('.')),
                        Extension = file.Extension.Remove(0, 1),
                        Content = s.ReadToEnd()
                    };
                }
            }
            catch
            {
                // File invalid
            }
            return result ?? new FileViewModel() { FileFolder = FileFolder };
        }

        public FileViewModel GetFile(string name, string ext, string FileFolder, bool isCreate = false, string defaultContent = "")
        {
            FileViewModel result = null;

            string fullPath = Path.Combine(CurrentDirectory, FileFolder, string.Format("{0}{1}", name, ext));

            FileInfo file = new FileInfo(fullPath);

            if (file.Exists)
            {
                try
                {
                    using (StreamReader s = file.OpenText())
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
                catch
                {
                    // File invalid
                }
            }
            else if (isCreate)
            {
                file.Create();
                result = new FileViewModel()
                {
                    FileFolder = FileFolder,
                    Filename = name,
                    Extension = ext,
                    Content = defaultContent
                };
            }

            return result ?? new FileViewModel() { FileFolder = FileFolder };
        }

        public FileViewModel GetFile(string fullname, string FileFolder, bool isCreate = false, string defaultContent = "")
        {
            var arr = fullname.Split('.');
            if (arr.Length == 2)
            {
                return GetFile(arr[0], $".{arr[1]}", FileFolder, isCreate, defaultContent);
            }
            else
            {
                return new FileViewModel() { FileFolder = FileFolder };
            }
        }

        public bool DeleteFile(string name, string extension, string FileFolder)
        {
            string folder = CommonHelper.GetFullPath(new string[] { MixConstants.Folder.UploadFolder, FileFolder });
            string fullPath = string.Format(@"{0}/{1}{2}", folder, name, extension);

            if (File.Exists(fullPath))
            {
                CommonHelper.RemoveFile(fullPath);
            }
            return true;
        }

        public bool DeleteFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                CommonHelper.RemoveFile(fullPath);
            }
            return true;
        }

        public bool DeleteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
                return true;
            }
            return false;
        }

        public bool EmptyFolder(string folderPath)
        {
            DeleteFolder(folderPath);
            CreateDirectoryIfNotExist(folderPath);
            return true;
        }

        public bool CopyDirectory(string srcPath, string desPath)
        {
            if (srcPath != desPath)
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
         public bool CopyWebDirectory(string srcPath, string desPath)
        {
            if (srcPath != desPath)
            {
                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories($"{MixConstants.Folder.WebRootPath}/{srcPath}", "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(srcPath, desPath));
                }

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles($"{MixConstants.Folder.WebRootPath}/{srcPath}", "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(srcPath, desPath), true);
                }

                return true;
            }
            return true;
        }

        private void CreateDirectoryIfNotExist(string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        public List<FileViewModel> GetUploadFiles(string folder)
        {
            string fullPath = CommonHelper.GetFullPath(new string[] { MixConstants.Folder.UploadFolder, folder });

            CreateDirectoryIfNotExist(fullPath);

            DirectoryInfo d = new DirectoryInfo(fullPath); //Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles();
            List<FileViewModel> result = new List<FileViewModel>();
            foreach (var file in Files.OrderByDescending(f => f.CreationTimeUtc))
            {
                using (StreamReader s = file.OpenText())
                {
                    result.Add(new FileViewModel()
                    {
                        FileFolder = folder,
                        Filename = file.Name.Substring(0, file.Name.LastIndexOf('.')),
                        Extension = file.Extension,
                        Content = s.ReadToEnd()
                    });
                }
            }
            return result;
        }

        public List<string> GetTopDirectories(string folder)
        {
            string fullPath = CommonHelper.GetFullPath(new string[]
            {
                MixConstants.Folder.WebRootPath,
                folder
            });
            List<string> result = new List<string>();
            if (Directory.Exists(fullPath))
            {
                foreach (string dirPath in Directory.GetDirectories(fullPath, "*",
                    SearchOption.TopDirectoryOnly))
                {
                    DirectoryInfo path = new DirectoryInfo(dirPath);
                    result.Add(path.Name);
                }
            }
            return result;
        }

        public List<FileViewModel> GetTopFiles(string folder)
        {
            string fullPath = CommonHelper.GetFullPath(new string[]
             {
                MixConstants.Folder.WebRootPath,
                //MixConstants.Folder.FileFolder,
                folder
             });

            string webPath = CommonHelper.GetFullPath(new string[]
             {
                //MixConstants.Folder.FileFolder,
                folder
             });
            List<FileViewModel> result = new List<FileViewModel>();
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo path = new DirectoryInfo(fullPath);
                string folderName = path.Name;

                var Files = path.GetFiles();
                foreach (var file in Files.OrderByDescending(f => f.CreationTimeUtc))
                {
                    result.Add(new FileViewModel()
                    {
                        FolderName = folderName,
                        FileFolder = webPath,

                        Filename = file.Name.Substring(0, file.Name.LastIndexOf('.') >= 0 ? file.Name.LastIndexOf('.') : 0),
                        Extension = file.Extension,
                        //Content = s.ReadToEnd()
                    });
                }
            }
            return result;
        }

        public List<FileViewModel> GetFilesWithContent(string fullPath)
        {
            CreateDirectoryIfNotExist(fullPath);

            //DirectoryInfo d = new DirectoryInfo(fullPath); //Assuming Test is your Folder
            FileInfo[] Files;
            List<FileViewModel> result = new List<FileViewModel>();
            foreach (string dirPath in Directory.GetDirectories(fullPath, "*",
                SearchOption.AllDirectories))
            {
                DirectoryInfo path = new DirectoryInfo(dirPath);
                string folderName = path.Name;

                Files = path.GetFiles();
                foreach (var file in Files.OrderByDescending(f => f.CreationTimeUtc))
                {
                    using (StreamReader s = file.OpenText())
                    {
                        result.Add(new FileViewModel()
                        {
                            FolderName = folderName,
                            FileFolder = CommonHelper.GetFullPath(new string[] { fullPath, folderName }),
                            Filename = file.Name.Substring(0, file.Name.LastIndexOf('.')),
                            Extension = file.Extension,
                            Content = s.ReadToEnd()
                        });
                    }
                }
            }
            return result;
        }

        public List<FileViewModel> GetFiles(string fullPath)
        {
            CreateDirectoryIfNotExist(fullPath);

            FileInfo[] Files;
            List<FileViewModel> result = new List<FileViewModel>();
            foreach (string dirPath in Directory.GetDirectories(fullPath, "*",
                SearchOption.AllDirectories))
            {
                DirectoryInfo path = new DirectoryInfo(dirPath);
                string folderName = path.Name;

                Files = path.GetFiles();
                foreach (var file in Files.OrderByDescending(f => f.CreationTimeUtc))
                {
                    result.Add(new FileViewModel()
                    {
                        FolderName = folderName,
                        FileFolder = CommonHelper.GetFullPath(new string[] { fullPath, folderName }),
                        Filename = file.Name.Substring(0, file.Name.LastIndexOf('.')),
                        Extension = file.Extension,
                        //Content = s.ReadToEnd()
                    });
                }
            }
            return result;
        }

        public List<FileViewModel> GetWebFiles(string folder)
        {
            string fullPath = CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.WebRootPath,
                    MixConstants.Folder.FileFolder,
                    folder
                });

            CreateDirectoryIfNotExist(fullPath);

            FileInfo[] Files;
            List<FileViewModel> result = new List<FileViewModel>();
            foreach (string dirPath in Directory.GetDirectories(fullPath, "*",
                SearchOption.AllDirectories))
            {
                DirectoryInfo path = new DirectoryInfo(dirPath);
                string folderName = path.ToString().Replace(@"\", "/").Replace(MixConstants.Folder.WebRootPath, string.Empty);

                Files = path.GetFiles();
                foreach (var file in Files.OrderByDescending(f => f.CreationTimeUtc))
                {
                    result.Add(new FileViewModel()
                    {
                        FolderName = path.Name,
                        FileFolder = folderName,
                        Filename = file.Name.LastIndexOf('.') >= 0 ? file.Name.Substring(0, file.Name.LastIndexOf('.'))
                                    : file.Name,
                        Extension = file.Extension
                    });
                }
            }
            return result;
        }

        public List<FileViewModel> GetFiles(MixEnums.FileFolder FileFolder)
        {
            string folder = FileFolder.ToString();
            return GetUploadFiles(folder);
        }

        public bool SaveWebFile(FileViewModel file)
        {
            try
            {
                string fullPath = CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.WebRootPath,
                    file.FileFolder
                });
                if (!string.IsNullOrEmpty(file.Filename))
                {
                    CreateDirectoryIfNotExist(fullPath);

                    string fileName = CommonHelper.GetFullPath(new string[] { fullPath, file.Filename + file.Extension });
                    if (File.Exists(fileName))
                    {
                        DeleteFile(fileName);
                    }
                    if (!string.IsNullOrEmpty(file.Content))
                    {
                        using (var writer = File.CreateText(fileName))
                        {
                            writer.WriteLine(file.Content); //or .Write(), if you wish
                            return true;
                        }
                    }
                    else
                    {
                        string base64 = file.FileStream.Split(',')[1];
                        byte[] bytes = Convert.FromBase64String(base64);
                        using (var writer = File.Create(fileName))
                        {
                            writer.Write(bytes, 0, bytes.Length);
                            return true;
                        }
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

        public string SaveFile(IFormFile file, string fullPath)
        {
            try
            {
                if (file.Length > 0)
                {
                    CreateDirectoryIfNotExist(fullPath);

                    string filename = file.FileName;
                    string filePath = CommonHelper.GetFullPath(new string[] { fullPath, filename });
                    if (File.Exists(filePath))
                    {
                        DeleteFile(filePath);
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return filename;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool SaveFile(FileViewModel file)
        {
            try
            {
                if (!string.IsNullOrEmpty(file.Filename))
                {
                    CreateDirectoryIfNotExist(file.FileFolder);

                    string fileName = $"{file.Filename}{file.Extension}";
                    if (!string.IsNullOrEmpty(file.FileFolder))
                    {
                        fileName = CommonHelper.GetFullPath(new string[] { file.FileFolder, fileName });
                    }
                    if (File.Exists(fileName))
                    {
                        DeleteFile(fileName);
                    }
                    if (!string.IsNullOrEmpty(file.Content))
                    {
                        using (var writer = File.CreateText(fileName))
                        {
                            writer.WriteLine(file.Content); //or .Write(), if you wish
                            writer.Dispose();
                            return true;
                        }
                    }
                    else
                    {
                        string base64 = file.FileStream.Split(',')[1];
                        byte[] bytes = Convert.FromBase64String(base64);
                        using (var writer = File.Create(fileName))
                        {
                            writer.Write(bytes, 0, bytes.Length);
                            return true;
                        }
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

        public string SaveWebFile(IFormFile file, string folder)
        {
            try
            {
                string fullPath = CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.WebRootPath,
                    folder
                });
                return SaveFile(file, fullPath);
            }
            catch
            {
                return string.Empty;
            }
        }

        public void UnZipFile(string fileName, string folder)
        {
            string filePath = CommonHelper.GetFullPath(new string[] {
                MixConstants.Folder.WebRootPath,
                folder,
                fileName });
            string webFolder = CommonHelper.GetFullPath(new string[]
            {
                MixConstants.Folder.WebRootPath,
                folder
            });
            try
            {
                ZipFile.ExtractToDirectory(filePath, webFolder);
            }
            catch
            {
                throw;
            }
        }

        public bool UnZipFile(FileViewModel file)
        {
            string filePath = CommonHelper.GetFullPath(new string[]
            {
                 MixConstants.Folder.WebRootPath,
                file.FileFolder,
                $"{file.Filename}{file.Extension}"
            });
            string webFolder = CommonHelper.GetFullPath(new string[]
            {
                MixConstants.Folder.WebRootPath,
                file.FileFolder
            });
            try
            {
                ZipFile.ExtractToDirectory(filePath, webFolder);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string ZipFolder(string srcFolder, string fileName)
        {
            try
            {
                string tmpPath = $"{MixConstants.Folder.WebRootPath}/{srcFolder}/tmp";
                string outputFile = $"{srcFolder}/{fileName}.zip";
                string outputFilePath = $"{MixConstants.Folder.WebRootPath}/{outputFile}";

                if (Directory.Exists(srcFolder))
                {
                    CopyDirectory(srcFolder, tmpPath);
                    if (File.Exists(outputFilePath))
                    {
                        File.Delete(outputFilePath);
                    }
                    ZipFile.CreateFromDirectory(tmpPath, outputFilePath);
                    DeleteFolder(tmpPath);
                    return outputFile;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}