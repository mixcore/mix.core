using Microsoft.AspNetCore.Http;
using Mix.Theme.Domain.ViewModels.Init;

namespace Mix.Theme.Domain.Dtos
{
    public class InitThemePackageDto
    {
        public InitThemeViewModel Model { get; set; }
        public IFormFile Assets { get; set; }
        public IFormFile Theme { get; set; }
    }
}
