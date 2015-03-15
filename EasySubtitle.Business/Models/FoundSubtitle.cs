using OSDBnet;

namespace EasySubtitle.Business.Models
{
    public class FoundSubtitle
    {
        public string SubtitleName
        {
            get { return Subtitle.SubtitleFileName; }
        }

        public bool Checked { get; set; }

        public Subtitle Subtitle { get; set; }
    }
}