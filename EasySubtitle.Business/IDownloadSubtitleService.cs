using System.Collections.Generic;
using System.Threading.Tasks;
using OSDBnet;

namespace EasySubtitle.Business
{
    public interface IDownloadSubtitleService
    {
        void DownloadSubtitle(Subtitle subtitle, string downlaodPath, string filePathForAdjustment = null);

        Task DownloadSubtitleAsync(Subtitle subtitle, string downlaodPath, string filePathForAdjustment = null);

        void DownloadSubtitle(IAnonymousClient client, Subtitle subtitle, string downlaodPath, string filePathForAdjustment = null);

        void DownloadSubtitles(IEnumerable<Subtitle> subtitles, string downlaodPath);
        
        void DownloadSubtitles(IAnonymousClient client, IEnumerable<Subtitle> subtitles, string downlaodPath);
    }
}