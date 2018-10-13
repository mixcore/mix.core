using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels
{
    public class GlobalSettingsViewModel
    {
        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("langIcon")]
        public string LangIcon { get; set; }

        [JsonProperty("themeId")]
        public int ThemeId { get; set; }
        
        [JsonProperty("apiEncryptKey")]
        public string ApiEncryptKey { get; set; }
          
        [JsonProperty("apiEncryptIV")]
        public string ApiEncryptIV { get; set; }

        [JsonProperty("isEncryptApi")]
        public bool IsEncryptApi { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        [JsonProperty("pageTypes")]
        public List<string> PageTypes { get; set; }

        [JsonProperty("statuses")]
        public List<string> Statuses { get; set; }
    }
    public class FilePageViewModel
    {
        [JsonProperty("files")]
        public List<FileViewModel> Files { get; set; }
        [JsonProperty("directories")]
        public List<string> Directories { get; set; }
    }
    public class InitCulture
    {
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("alias")]
        public string Alias { get; set; }
    }
    public class ExtraProperty
    {
        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

    }

    public class DataValueViewModel
    {
        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class FileViewModel
    {
        private string _fullPath = string.Empty;
        private string _webPath = string.Empty;
        [JsonProperty("fullPath")]
        public string FullPath
        {
            get
            {
                _fullPath = CommonHelper.GetFullPath(new string[] {
                    FileFolder,
                    FolderName,
                    $"{Filename}{Extension}"
                });

                return _fullPath;
            }
            set
            {
                _fullPath = value;
            }
        }
        [JsonProperty("webPath")]
        public string WebPath
        {
            get
            {
                _webPath = CommonHelper.GetFullPath(new string[] {
                    FileFolder,
                    $"{Filename}{Extension}"
                });
                return _webPath;
            }
            set
            {
                _webPath = value;
            }
        }
        [JsonProperty("folderName")]
        public string FolderName { get; set; }
        [JsonProperty("fileFolder")]
        public string FileFolder { get; set; }
        [JsonProperty("fileName")]
        public string Filename { get; set; }
        [JsonProperty("extension")]
        public string Extension { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("fileStream")]
        public string FileStream { get; set; }
    }

    public class TemplateViewModel
    {
        public string FileFolder { get; set; }

        [Required]
        public string Filename { get; set; }

        public string Extension { get; set; }
        public string Content { get; set; }
        public string Scripts { get; set; }
        public string Styles { get; set; }
        public string FileStream { get; set; }
    }

    public class ModuleFieldViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("options")]
        public JArray Options { get; set; } = new JArray();

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }

        [JsonProperty("isUnique")]
        public bool IsUnique { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequired { get; set; }

        [JsonProperty("isDisplay")]
        public bool IsDisplay { get; set; }

        [JsonProperty("isSelect")]
        public bool IsSelect { get; set; }

        [JsonProperty("isGroupBy")]
        public bool IsGroupBy { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }

    public class ApiModuleDataValueViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("isUnique")]
        public bool IsUnique { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequired { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("isDisplay")]
        public bool IsDisplay { get; set; }
        [JsonProperty("isSelect")]
        public bool IsSelect { get; set; }
        [JsonProperty("isGroupBy")]
        public bool IsGroupBy { get; set; }
        [JsonProperty("options")]
        public JArray Options { get; set; } = new JArray();

        public RepositoryResponse<bool> Validate<T>(IConvertible id, string specificulture, JObject jItem, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where T : class
        {

            string val = jItem[Name]["value"].Value<string>();
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (IsUnique)
            {
                string query = @"SELECT * FROM [Mix_module_data] WHERE JSON_VALUE([Value],'$.{0}.value') = '{1}'"; // AND Specificulture = '{2}' AND Id <> '{3}'
                var temp = string.Format(query, Name, val);//, specificulture, id?.ToString()
                int count = _context.MixModuleData.FromSql(query, Name, val).Count(d=>d.Specificulture == specificulture && d.Id != id.ToString());//, specificulture, id?.ToString()

                //string query = $"SELECT * FROM Mix_module_data WHERE JSON_VALUE([Value],'$.{Name}.value') = '{val}' AND Specificulture = '{specificulture}' AND Id != '{id}'";
                //int count = _context.MixModuleData.FromSql(sql: new RawSqlString(query)).Count();
                if (count > 0)
                {
                    result.IsSucceed = false;
                    result.Errors.Add($"{Title} is existed");
                }
            }
            if (IsRequired)
            {
                if (string.IsNullOrEmpty(val))
                {
                    result.IsSucceed = false;
                    result.Errors.Add($"{Title} is required");
                }
            }
            return result;
        }
    }

}
