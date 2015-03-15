using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSDBnet;

namespace EasySubtitle.Business
{
    public class DownloadSubtitleService : IDownloadSubtitleService
    {
        private readonly SubtitleClientCredentials _credentials;

        public DownloadSubtitleService(SubtitleClientCredentials credentials)
        {
            _credentials = credentials;
        }

        public void DownloadSubtitle(Subtitle subtitle, string downlaodPath, string filePathForAdjustment = null)
        {
            using (var client = SubtitleClientFactory.GetSubtitleClient(_credentials))
            {
                DownloadSubtitle(client, subtitle, downlaodPath, filePathForAdjustment);
            }
        }

        public void DownloadSubtitle(IAnonymousClient client, Subtitle subtitle, string downlaodPath, string filePathForAdjustment = null)
        {
            if (client == null) throw new ArgumentNullException("subtitles");
            if (subtitle == null) throw new ArgumentNullException("subtitles");
            if (String.IsNullOrWhiteSpace(downlaodPath)) throw new ArgumentNullException("downlaodPath");

            client.DownloadSubtitleToPath(downlaodPath, subtitle);

            if (string.IsNullOrWhiteSpace(filePathForAdjustment))
                return;

            var subtitleFileNameToCreate = GetFullSubtitleFileNameToMatchMediaFile(filePathForAdjustment, downlaodPath);
            File.Delete(subtitleFileNameToCreate);
            File.Move(GetFullSubtitleFileName(downlaodPath, subtitle), subtitleFileNameToCreate);
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

            if (!subtitles.Any())
                return;

            subtitles.ToList().ForEach(subtitle => DownloadSubtitle(client, subtitle, downlaodPath));
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