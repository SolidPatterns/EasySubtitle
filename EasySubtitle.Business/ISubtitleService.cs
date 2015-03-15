using System.Collections.Generic;
using OSDBnet;

namespace EasySubtitle.Business
{
    public interface ISubtitleService
    {
        IDictionary<string, IEnumerable<Subtitle>> FindSubtitles(IEnumerable<string> mediaFiles, params string[] languages);
        IEnumerable<Subtitle> FindSubtitles(string filePath, params string[] languages);
    }
}