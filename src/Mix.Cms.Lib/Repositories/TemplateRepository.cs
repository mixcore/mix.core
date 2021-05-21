// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mix.Cms.Lib.Repositories
{
    public class TemplateRepository
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static volatile TemplateRepository instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static TemplateRepository Instance {
            get {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TemplateRepository();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TemplateRepository"/> class from being created.
        /// </summary>
        private TemplateRepository()
        {
        }

        public TemplateViewModel GetTemplate(string templatePath, List<TemplateViewModel> templates, string templateFolder)
        {
            var result = templates.Find(v => !string.IsNullOrEmpty(templatePath) && v.Filename == templatePath.Replace(@"\", "/").Split('/')[1]);
            return result ?? new TemplateViewModel() { FileFolder = templateFolder };
        }

        public TemplateViewModel GetTemplate(string name, string templateFolder)
        {
            DirectoryInfo d = new DirectoryInfo(templateFolder);
            FileInfo[] Files = d.GetFiles(name); //Getting cshtml files
            var file = Files.FirstOrDefault();
            TemplateViewModel result = null;
            if (file != null)
            {
                using (StreamReader s = file.OpenText())
                {
                    result = new TemplateViewModel()
                    {
                        FileFolder = templateFolder,
                        Filename = file.Name,
                        Extension = file.Extension,
                        Content = s.ReadToEnd()
                    };
                }
            }
            return result ?? new TemplateViewModel() { FileFolder = templateFolder };
        }

        public bool DeleteTemplate(string name, string templateFolder)
        {
            string fullPath = $"{templateFolder}/{name + MixFileExtensions.CsHtml}";

            if (File.Exists(fullPath))
            {
                MixCommonHelper.RemoveFile(fullPath);
            }

            return true;
        }

        public List<TemplateViewModel> GetTemplates(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            DirectoryInfo d = new DirectoryInfo(folder);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(string.Format("*{0}", MixFileExtensions.CsHtml)); //Getting cshtml files
            List<TemplateViewModel> result = new List<TemplateViewModel>();
            foreach (var file in Files)
            {
                using (StreamReader s = file.OpenText())
                {
                    result.Add(new TemplateViewModel()
                    {
                        FileFolder = folder,
                        Filename = file.Name,
                        Extension = MixFileExtensions.CsHtml,
                        Content = s.ReadToEnd()
                    });
                }
            }
            return result;
        }

        public bool SaveTemplate(TemplateViewModel file)
        {
            try
            {
                if (!string.IsNullOrEmpty(file.Filename))
                {
                    if (!Directory.Exists(file.FileFolder))
                    {
                        Directory.CreateDirectory(file.FileFolder);
                    }
                    string fileName = $"{file.FileFolder}/{file.Filename + file.Extension}";
                    using (var writer = File.CreateText(fileName))
                    {
                        writer.WriteLine(file.Content);
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
    }
}