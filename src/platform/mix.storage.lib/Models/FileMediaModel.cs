using Mix.Storage.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Storage.Lib.Models
{
    public sealed class FileMediaModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public long Size { get; set; }
        public string Mimetype { get; set; }
        public DateTime UploadDate { get; set; }

        public FileMediaModel(MixMediaViewModel media)
        {
            Id = media.Id;
            Url = media.TargetUrl;
            Size = media.FileSize;
            Mimetype = media.FileType;
            UploadDate = media.CreatedDateTime;
        }

    }
}
