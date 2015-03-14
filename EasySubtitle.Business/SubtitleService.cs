using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSDBnet;

namespace EasySubtitle.Business
{
    //todo: make this service return found subtitle dto's
    //todo: create a download service that downloads given subtitles
    //todo: create a subtitle engine that finds subtitles and downloads them if asked to
    
    public class SubtitleService : ISubtitleService
    {
        private readonly SubtitleServiceCredentials _credentials;

        public SubtitleService(SubtitleServiceCredentials credentials)
        {
            _credentials = credentials;
        }

        public void FindSubtitles(IEnumerable<string> mediaFiles, params string[] languages)
        {
            var filePaths = mediaFiles as IList<string> ?? mediaFiles.ToList();

            if (filePaths == null || !filePaths.Any()) throw new ArgumentNullException("mediaFiles");

            using (var client = Osdb.Login(_credentials.UserAgent))
            {
                foreach (var filePath in filePaths)
                {
                    FindSubtitles(client, filePath);
                }
            }
        }

        public void FindSubtitles(string filePath, params string[] languages)
        {
            using (var client = Osdb.Login(_credentials.UserAgent))
            {
                FindSubtitles(client, filePath, languages);
            }
        }

        private void FindSubtitles(IAnonymousClient client, string filePath, params string[] languages)
        {
            if (languages == null || !languages.Any()) throw new ArgumentNullException("languages");

            foreach (var language in languages)
            {
                FindSubtitleByLanguage(client, filePath, language);
            }
        }

        private static void FindSubtitleByLanguage(IAnonymousClient client, string filePath, string language)
        {
            var subtitles = client.SearchSubtitlesFromFile(language, filePath);
            if (!subtitles.Any())
                return;

            var subtitle = subtitles.OrderByDescending(x => x.Rating).FirstOrDefault();
            if (subtitle == null)
                return;

            var directoryPath = Path.GetDirectoryName(filePath);
            client.DownloadSubtitleToPath(directoryPath, subtitle);
            File.Move(GetFullSubtitleFileName(directoryPath, subtitle),
                GetFullSubtitleFileNameToMatchMediaFile(filePath, directoryPath));
        }

        private static string GetFullSubtitleFileNameToMatchMediaFile(string filePath, string directoryPath)
        {
            return String.Concat(directoryPath, Path.DirectorySeparatorChar.ToString(),
                Path.GetFileNameWithoutExtension(filePath), ".srt");
        }

        private static string GetFullSubtitleFileName(string directoryPath, Subtitle subtitle)
        {
            return String.Concat(directoryPath, Path.DirectorySeparatorChar.ToString(), subtitle.SubtitleFileName);
        }
    }
}