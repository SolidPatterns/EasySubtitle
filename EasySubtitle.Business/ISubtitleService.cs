using System.Collections.Generic;
using System.Threading.Tasks;
using OSDBnet;

namespace EasySubtitle.Business
{
    public interface ISubtitleService
    {
        IEnumerable<Subtitle> FindSubtitles(IAnonymousClient client, string filePath, params string[] languages);

        IEnumerable<Subtitle> FindSubtitles(string filePath, params string[] languages);

        IDictionary<string, IEnumerable<Subtitle>> FindSubtitles(IEnumerable<string> mediaFiles, params string[] languages);

        Task<IEnumerable<Subtitle>> FindSubtitlesAsync(string filePath, string language);

        /// <summary>
        /// Downloads the given subtitle and renames it according to the media file name.
        /// </summary>
        /// <param name="subtitle"></param>
        /// <param name="subtitleMediaFilePath"></param>
        void DownloadSubtitleAdjusted(Subtitle subtitle, string subtitleMediaFilePath);

        /// <summary>
        /// Downloads the given subtitle and renames it according to the media file name.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="subtitle"></param>
        /// <param name="subtitleMediaFilePath"></param>
        void DownloadSubtitleAdjusted(IAnonymousClient client, Subtitle subtitle, string subtitleMediaFilePath);

        Task DownloadSubtitleAdjustedAsync(Subtitle subtitle, string subtitleMediaFilePath);
        
        Task DownloadSubtitleAsync(Subtitle subtitle, string downlaodPath);
        
        void DownloadSubtitle(IAnonymousClient client, Subtitle subtitle, string downlaodPath);
        
        void DownloadSubtitle(Subtitle subtitle, string downlaodPath);

        void DownloadSubtitles(IEnumerable<Subtitle> subtitles, string downlaodPath);

        void DownloadSubtitles(IAnonymousClient client, IEnumerable<Subtitle> subtitles, string downlaodPath);

        
    }
}