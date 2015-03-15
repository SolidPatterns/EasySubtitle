using OSDBnet;

namespace EasySubtitle.Business.Models
{
    public class FoundSubtitle
    {
        public string SubtitleName { get; set; }
        public bool Checked { get; set; }

        public Subtitle Subtitle { get; set; }
    }
}