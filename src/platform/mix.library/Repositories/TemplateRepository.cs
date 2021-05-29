using Mix.Common.Helper;
using Mix.Lib.Constants;
using Mix.Lib.Models.Common;
using Mix.Lib.ViewModels.Cms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mix.Lib.Repositories
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
        public static TemplateRepository Instance
        {
            get
            {
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

        public TemplateModel GetTemplate(string templatePath, List<TemplateModel> templates, string templateFolder)
        {
            var result = templates.Find(v => !string.IsNullOrEmpty(templatePath) && v.Filename == templatePath.Replace(@"\", "/").Split('/')[1]);
            return result ?? new TemplateModel() { FileFolder = templateFolder };
        }

        public TemplateModel GetTemplate(string name, string templateFolder)
        {
            DirectoryInfo d = new DirectoryInfo(templateFolder);
            FileInfo[] Files = d.GetFiles(name); //Getting cshtml files
            var file = Files.FirstOrDefault();
            TemplateModel result = null;
            if (file != null)
            {
                using (StreamReader s = file.OpenText())
                {
                    result = new TemplateModel()
                    {
                        FileFolder = templateFolder,
                        Filename = file.Name,
                        Extension = file.Extension,
                        Content = s.ReadToEnd()
                    };
                }
            }
            return result ?? new TemplateModel() { FileFolder = templateFolder };
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

        public List<TemplateModel> GetTemplates(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            DirectoryInfo d = new DirectoryInfo(folder);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(string.Format("*{0}", MixFileExtensions.CsHtml)); //Getting cshtml files
            List<TemplateModel> result = new List<TemplateModel>();
            foreach (var file in Files)
            {
                using (StreamReader s = file.OpenText())
                {
                    result.Add(new TemplateModel()
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

        public bool SaveTemplate(TemplateModel file)
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
