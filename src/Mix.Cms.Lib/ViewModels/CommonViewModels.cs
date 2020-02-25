using Microsoft.AspNetCore.Http;
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
using System.Xml.Linq;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels
{
    public class GlobalSettingsViewModel
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("langIcon")]
        public string LangIcon { get; set; }

        [JsonProperty("themeId")]
        public int ThemeId { get; set; }

        [JsonProperty("portalThemeSettings")]
        public JObject PortalThemeSettings { get; set; }

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

        [JsonProperty("moduleTypes")]
        public List<string> ModuleTypes { get; set; }

        [JsonProperty("attributeSetTypes")]
        public List<string> AttributeSetTypes { get; set; }

        [JsonProperty("dataTypes")]
        public List<string> DataTypes { get; set; }

        [JsonProperty("statuses")]
        public List<string> Statuses { get; set; }

        [JsonProperty("lastUpdateConfiguration")]
        public DateTime? LastUpdateConfiguration { get; set; }
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
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    //public class CryptoViewModel<T>
    //{
    //    [JsonProperty("base64Key")]
    //    public string Base64Key { get; set; }
    //    [JsonProperty("base64IV")]
    //    public string Base64IV { get; set; }
    //    [JsonProperty("data")]
    //    public T Data { get; set; }
    //}
    public class DataValueViewModel
    {
        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; } = MixDataType.Text;

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
        public string FullPath {
            get {
                _fullPath = CommonHelper.GetFullPath(new string[] {
                    FileFolder,
                    //FolderName,
                    $"{Filename}{Extension}"
                });

                return _fullPath;
            }
            set {
                _fullPath = value;
            }
        }

        [JsonProperty("webPath")]
        public string WebPath {
            get {
                _webPath = FullPath.Replace("wwwroot", string.Empty);
                return _webPath;
            }
            set {
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

        public FileViewModel()
        {
        }

        public FileViewModel(IFormFile file, string folder)
        {
            Filename = file.FileName.Substring(0, file.FileName.LastIndexOf('.'));
            Extension = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            FileFolder = folder;
        }
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

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

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
            var jVal = new JProperty(Name, jItem[Name]);
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (IsUnique)
            {
                //string query = @"SELECT * FROM [Mix_module_data] WHERE JSON_VALUE([Value],'$.{0}.value') = '{1}'"; // AND Specificulture = '{2}' AND Id <> '{3}'
                //var temp = string.Format(query, Name, val);//, specificulture, id?.ToString()
                //int count = _context.MixModuleData.FromSql(query, Name, val).Count(d=>d.Specificulture == specificulture && d.Id != id.ToString());//, specificulture, id?.ToString()
                //string query = $"SELECT * FROM Mix_module_data WHERE JSON_VALUE([Value],'$.{Name}.value') = '{val}' AND Specificulture = '{specificulture}' AND Id != '{id}'";
                //int count = _context.MixModuleData.FromSql(sql: new RawSqlString(query)).Count();
                var strId = id?.ToString();
                int count = _context.MixModuleData.Count(d => d.Specificulture == specificulture
                    && d.Value.Contains(jVal.ToString(Formatting.None)) && d.Id != strId);
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

    public class MobileComponent
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("componentType")]
        public string ComponentType { get; set; }

        [JsonProperty("styleName")]
        public string StyleName { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("dataValue")]
        public string DataValue { get; set; }

        [JsonProperty("dataSource")]
        public List<MobileComponent> DataSource { get; set; }

        public MobileComponent(XElement element)
        {
            if (element != null)
            {
                StyleName = element.Attribute("class")?.Value;

                DataSource = new List<MobileComponent>();
                var subElements = element.Elements();
                if (subElements.Any())
                {
                    if (element.Attribute("data") != null)
                    {
                        ComponentType = "View";
                        DataValue = element.Attribute("data")?.Value.Replace("Model.", "@Model.").Replace("{{", "").Replace("}}", "");
                        DataType = "object_array";
                    }
                    else
                    {
                        ComponentType = "View";
                        DataType = "component";
                    }
                    foreach (var subElement in subElements)
                    {
                        if (subElement.Name != "br")
                        {
                            DataSource.Add(new MobileComponent(subElement));
                        }
                    }
                }
                else
                {
                    switch (element.Name.LocalName)
                    {
                        case "img":
                            ComponentType = "Image";
                            DataType = "image_url";
                            DataValue = element.Attribute("src")?.Value.Replace("Model.", "@Model.").Replace("{{", "").Replace("}}", "");
                            break;

                        case "br":
                            break;

                        default:
                            ComponentType = "Text";

                            string val = element.Value.Trim();
                            if (val.Contains("{{") && val.Contains("}}"))
                            {
                                DataType = "object";
                            }
                            else
                            {
                                DataType = "string";
                            }
                            DataValue = element.Value.Trim().Replace("Model.", "@Model.").Replace("{{", "").Replace("}}", "");
                            break;
                    }
                }
            }
        }
    }

    public class SiteMap
    {
        public DateTime? LastMod { get; set; }
        public string ChangeFreq { get; set; }
        public double Priority { get; set; }
        public string Loc { get; set; }
        public List<SitemapLanguage> OtherLanguages { get; set; }

        public XElement ParseXElement()
        {
            XNamespace xhtml = "http://www.w3.org/1999/xhtml";
            XNamespace ns = @"http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace xsi = @"http://www.w3.org/1999/xhtml";

            var e = new XElement(ns + "url");
            e.Add(new XElement(ns + "lastmod", LastMod.HasValue ? LastMod.Value : DateTime.UtcNow));
            e.Add(new XElement(ns + "changefreq", ChangeFreq));
            e.Add(new XElement(ns + "priority", Priority));
            e.Add(new XElement(ns + "loc", Loc));
            foreach (var item in OtherLanguages)
            {
                e.Add(new XElement(xsi + "link",
                     new XAttribute(XNamespace.Xmlns + "xhtml", xsi.NamespaceName),
                    new XAttribute("rel", "alternate"),
                    new XAttribute("hreflang", item.HrefLang),
                    new XAttribute("href", item.Href)
                    ));
            }
            return e;
        }
    }

    public class ListAction<T>
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }

    public class SitemapLanguage
    {
        public string HrefLang { get; set; }
        public string Href { get; set; }
    }

    public class FileInputModel
    {
        public IFormFile FileToUpload { get; set; }
    }
}