using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Mix.Cms.Lib.ViewModels.Common
{
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
}