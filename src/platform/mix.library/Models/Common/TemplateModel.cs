using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.Models.Common
{
    public class TemplateModel
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
}
