using System.Collections.Generic;

namespace EasySubtitle.Business
{
    public interface ISubtitleService
    {
        void FindSubtitles(IEnumerable<string> mediaFiles, params string[] languages);
        void FindSubtitles(string filePath, params string[] languages);
    }
}