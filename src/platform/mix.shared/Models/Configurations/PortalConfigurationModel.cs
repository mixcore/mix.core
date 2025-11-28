using Mix.Heart.Enums;

namespace Mix.Shared.Models.Configurations
{
    public class PortalConfigurationModel
    {
        public int PrimaryColorHue { get; set; }
        public int PrimaryColorSaturation { get; set; }
        public int PrimaryColorBrightness { get; set; }
        public string BgColor { get; set; }
        public string TextColor { get; set; }
        public string PrimaryColor { get; set; }
        public string BgColorHover { get; set; }
        public string BorderColor { get; set; }
        public string BorderColorHover { get; set; }
        public string LinkColor { get; set; }
        public string LinkColorHover { get; set; }
        public string LinkColorActive { get; set; }
        public string TextColorHover { get; set; }
        public string FontFamily { get; set; }
        public string FontSizeH1 { get; set; }
        public string FontSize { get; set; }
    }
}
