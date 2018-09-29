using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using Mix.Cms.Lib.Models.Cms;
using System.Collections.Generic;
using System.Linq;
using Mix.Domain.Core.Models;
using MixCore.Cms.Lib.ViewModels.System;
using Mix.Cms.Lib.Repositories;

namespace Mix.Cms.Lib.Services
{
    public class CmsCultureConfiguration
    {
        public JObject Translator { get; set; }
        public List<SystemLanguageViewModel> ListLanguage { get; set; }

        public CmsCultureConfiguration()
        {            
        }

        public SupportedCulture GetCulture(string specificulture)
        {
            var cultures = CommonRepository.Instance.LoadCultures();
            return cultures.Find(c => c.Specificulture == specificulture);
        }

        public List<SupportedCulture> GetSupportedCultures()
        {
            return CommonRepository.Instance.LoadCultures();
        }

        public void Init(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getLanguages = SystemLanguageViewModel.Repository.GetModelList(_context, _transaction);
            ListLanguage = getLanguages.Data ?? new List<SystemLanguageViewModel>();
            Translator = new JObject();
            foreach (var culture in GetSupportedCultures())
            {
                JObject arr = new JObject();
                foreach (var lang in ListLanguage.Where(l=>l.Specificulture== culture.Specificulture))
                {
                    JProperty l = new JProperty(lang.Keyword, lang.Value??lang.DefaultValue);
                    arr.Add(l);
                }
                Translator.Add(new JProperty(culture.Specificulture, arr));
            }
        }

        public string Translate(string culture, string key)
        {
            return Translator[culture][key]?.ToString() ?? key;
        }

        public bool UpdateLanguage(string key, string culture, string value)
        {
            var config = ListLanguage.Find(c => c.Keyword == key && c.Specificulture == culture);
            string oldValue = config.Value;

            config.Value = value;
            var result = SystemLanguageViewModel.Repository.SaveModel(config);

            if (result.IsSucceed)
            {
                Translator[culture][key] = value;
                return true;
            }
            else
            {
                config.Value = oldValue;
                return false;
            }
        }

        public string GetLocalString(string key, string culture)
        {
            var config = ListLanguage.Find(c => c.Keyword == key && c.Specificulture == culture);
            return config != null ? config.Value : key;
        }

        public string GetLocalString(string key, string culture, string defaultValue)
        {
            var config = ListLanguage.Find(c => c.Keyword == key && c.Specificulture == culture);
            return config != null ? config.Value : defaultValue;
        }

        public int GetLocalInt(string key, string culture, int defaultValue)
        {
            var config = ListLanguage.Find(c => c.Keyword == key && c.Specificulture == culture);
            if (!int.TryParse(config?.Value, out int result))
            {
                result = defaultValue;
            }
            return result;
        }
    }
    public class Translator
    {
        public string Culture { get; set; }

        public Translator(string culture)
        {
            this.Culture = culture;
        }

        public string Get(string key)
        {
            return MixCmsService.Instance.CmsCulture.Translator[Culture][key]?.ToString() ?? key;
        }
    }
}
