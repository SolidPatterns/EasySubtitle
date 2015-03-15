using System;
using System.Collections.Generic;
using System.Linq;
using OSDBnet;

namespace EasySubtitle.Business
{
    public class SubtitleService : ISubtitleService
    {
        private readonly SubtitleClientCredentials _credentials;

        public SubtitleService(SubtitleClientCredentials credentials = null)
        {
            _credentials = credentials;
        }

        public IDictionary<string, IEnumerable<Subtitle>> FindSubtitles(IEnumerable<string> mediaFiles, params string[] languages)
        {
            var filePaths = mediaFiles as IList<string> ?? mediaFiles.ToList();

            if (filePaths == null || !filePaths.Any()) throw new ArgumentNullException("mediaFiles");

            var subtitleDictionary = new Dictionary<string, IEnumerable<Subtitle>>();
            using (var client = SubtitleClientFactory.GetSubtitleClient(_credentials))
            {
                foreach (var filePath in filePaths)
                {
                    subtitleDictionary.Add(filePath, FindSubtitles(client, filePath, languages));
                }
            }

            return subtitleDictionary;
        }

        public IEnumerable<Subtitle> FindSubtitles(string filePath, params string[] languages)
        {
            using (var client = SubtitleClientFactory.GetSubtitleClient(_credentials))
            {
                return FindSubtitles(client, filePath, languages);
            }
        }

        private IEnumerable<Subtitle> FindSubtitles(IAnonymousClient client, string filePath, params string[] languages)
        {
            if (languages == null || !languages.Any()) throw new ArgumentNullException("languages");

            var subtitles = new List<Subtitle>();
            foreach (var language in languages)
            {
                var foundSubtitles = FindSubtitleByLanguage(client, filePath, language);
                if (foundSubtitles != null && foundSubtitles.Any())
                    subtitles.AddRange(foundSubtitles);
            }

            return subtitles;
        }

        private IEnumerable<Subtitle> FindSubtitleByLanguage(IAnonymousClient client, string filePath, string language)
        {
            if(client == null) throw new ArgumentNullException("client");
            if(string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException("filePath");
            if (string.IsNullOrWhiteSpace(language)) throw new ArgumentNullException("language");

            return client.SearchSubtitlesFromFile(language, filePath);
        }
    }
}