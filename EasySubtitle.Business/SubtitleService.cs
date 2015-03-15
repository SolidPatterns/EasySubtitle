using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public IDictionary<string, IEnumerable<Subtitle>> FindSubtitles(IEnumerable<string> mediaFiles, string[] languages)
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

        public IEnumerable<Subtitle> FindSubtitles(string filePath, string[] languages)
        {
            using (var client = SubtitleClientFactory.GetSubtitleClient(_credentials))
            {
                return FindSubtitles(client, filePath, languages);
            }
        }

        public IEnumerable<Subtitle> FindSubtitles(IAnonymousClient client, string filePath, string language)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException("filePath");
            if (string.IsNullOrWhiteSpace(language)) throw new ArgumentNullException("language");

            return client.SearchSubtitlesFromFile(language, filePath);
        }

        public IEnumerable<Subtitle> FindSubtitles(string filePath, string language)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException("filePath");
            if (string.IsNullOrWhiteSpace(language)) throw new ArgumentNullException("language");

            using (var client = SubtitleClientFactory.GetSubtitleClient(_credentials))
            {
                return FindSubtitles(client, filePath, language);
            }
        }

        public IEnumerable<Subtitle> FindSubtitles(IAnonymousClient client, string filePath, string[] languages)
        {
            if (languages == null || !languages.Any()) throw new ArgumentNullException("languages");

            var subtitles = new List<Subtitle>();
            foreach (var language in languages)
            {
                var foundSubtitles = FindSubtitles(client, filePath, language);
                var foundSubs = foundSubtitles as Subtitle[] ?? foundSubtitles.ToArray();
                if (foundSubtitles != null && foundSubs.Any())
                    subtitles.AddRange(foundSubs);
            }

            return subtitles;
        }

        public async Task<IEnumerable<Subtitle>> FindSubtitlesAsync(string filePath, string language)
        {
            return await Task.Factory.StartNew(() => FindSubtitles(filePath, language));
        }

        public void DownloadSubtitleAdjusted(Subtitle subtitle, string subtitleMediaFilePath)
        {
            var directoryPath = GetDirectoryPath(subtitleMediaFilePath);
            var filePathForAdjustment = GetFullSubtitleFileNameToMatchMediaFile(subtitleMediaFilePath);
            DownloadSubtitle(subtitle, directoryPath, filePathForAdjustment);
        }

        public void DownloadSubtitleAdjusted(IAnonymousClient client, Subtitle subtitle, string subtitleMediaFilePath)
        {
            var directoryPath = GetDirectoryPath(subtitleMediaFilePath);
            var filePathForAdjustment = GetFullSubtitleFileNameToMatchMediaFile(subtitleMediaFilePath);
            DownloadSubtitle(client, subtitle, directoryPath, filePathForAdjustment);
        }

        public async Task DownloadSubtitleAdjustedAsync(Subtitle subtitle, string subtitleMediaFilePath)
        {
            await Task.Factory.StartNew(() => DownloadSubtitle(subtitle, subtitleMediaFilePath));
        }

        public async Task DownloadSubtitleAsync(Subtitle subtitle, string downlaodPath)
        {
            await Task.Factory.StartNew(() => DownloadSubtitle(subtitle, downlaodPath));
        }

        public void DownloadSubtitle(IAnonymousClient client, Subtitle subtitle, string downlaodPath)
        {
            DownloadSubtitle(client, subtitle, downlaodPath, null);
        }

        public void DownloadSubtitle(Subtitle subtitle, string downlaodPath)
        {
            using (var client = SubtitleClientFactory.GetSubtitleClient(_credentials))
            {
                DownloadSubtitle(client, subtitle, downlaodPath);
            }
        }

        public void DownloadSubtitles(IEnumerable<Subtitle> subtitles, string downlaodPath)
        {
            using (var client = SubtitleClientFactory.GetSubtitleClient(_credentials))
            {
                DownloadSubtitles(client, subtitles, downlaodPath);
            }
        }

        public void DownloadSubtitles(IAnonymousClient client, IEnumerable<Subtitle> subtitles, string downlaodPath)
        {
            if (client == null) throw new ArgumentNullException("subtitles");
            if (subtitles == null) throw new ArgumentNullException("subtitles");
            if (String.IsNullOrWhiteSpace(downlaodPath)) throw new ArgumentNullException("downlaodPath");

            var subs = subtitles as Subtitle[] ?? subtitles.ToArray();
            if (!subs.Any())
                return;

            subs.ToList().ForEach(subtitle => DownloadSubtitle(client, subtitle, downlaodPath));
        }

        private void DownloadSubtitle(Subtitle subtitle, string downlaodPath, string filePathForAdjustment)
        {
            using (var client = SubtitleClientFactory.GetSubtitleClient(_credentials))
            {
                DownloadSubtitle(client, subtitle, downlaodPath, filePathForAdjustment);
            }
        }

        private static void DownloadSubtitle(IAnonymousClient client, Subtitle subtitle, string downlaodPath, string filePathForAdjustment)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (subtitle == null) throw new ArgumentNullException("subtitle");
            if (String.IsNullOrWhiteSpace(downlaodPath)) throw new ArgumentNullException("downlaodPath");

            client.DownloadSubtitleToPath(downlaodPath, subtitle);

            if (string.IsNullOrWhiteSpace(filePathForAdjustment))
                return;

            RenameSubtitle(subtitle, downlaodPath, filePathForAdjustment);
        }

        private static void RenameSubtitle(Subtitle subtitle, string downlaodPath, string filePathForAdjustment)
        {
            var subtitleFileNameToCreate = GetFullSubtitleFileNameToMatchMediaFile(filePathForAdjustment, downlaodPath);
            if (!subtitle.SubtitleFileName.Equals(subtitleFileNameToCreate.Split(Path.DirectorySeparatorChar).Last()))
                File.Delete(subtitleFileNameToCreate);
            
            var fullSubtitleFileName = GetFullSubtitleFileName(downlaodPath, subtitle);
            if (File.Exists(fullSubtitleFileName))
                File.Move(fullSubtitleFileName, subtitleFileNameToCreate);
        }

        private static string GetDirectoryPath(string subtitleMediaFilePath)
        {
            return Path.GetDirectoryName(subtitleMediaFilePath);
        }

        private static string GetFullSubtitleFileNameToMatchMediaFile(string filePath)
        {
            return GetFullSubtitleFileNameToMatchMediaFile(filePath, GetDirectoryPath(filePath));
        }

        private static string GetFullSubtitleFileNameToMatchMediaFile(string filePath, string directoryPath)
        {
            return String.Concat(directoryPath, Path.DirectorySeparatorChar,
                Path.GetFileNameWithoutExtension(filePath), ".srt");
        }

        private static string GetFullSubtitleFileName(string directoryPath, Subtitle subtitle)
        {
            return String.Concat(directoryPath, Path.DirectorySeparatorChar, subtitle.SubtitleFileName);
        }
    }
}