using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EasySubtitle.Business;
using NUnit.Framework;
using OSDBnet;

namespace EasySubtitle.Tests
{
    [TestFixture]
    public class DownloadSubtitleServiceShould
    {
        private ISubtitleService _subtitleService;
        private readonly List<string> mediaPaths = new List<string>
        {
            "F:/Videos/Series/Supernatural/Supernatural.S10E11.1080p.HDTV.X264-DIMENSION.mkv",
            "F:/Videos/Series/Supernatural/Supernatural.S10E13.1080p.HDTV.X264-DIMENSION.mkv",
            "F:/Videos/Series/Supernatural/Supernatural.S10E14.1080p.HDTV.X264-DIMENSION.mkv"
        };

        private IDownloadSubtitleService _downloadSubtitleService;

        [SetUp]
        public void SetUp()
        {
            _subtitleService = EasySubtitleFactory.Instance.GetSubtitleService();
            _downloadSubtitleService = EasySubtitleFactory.Instance.GetDownloadSubtitleService();
        }

        [Test]
        public void Download_Subtitle_To_Given_Directory_When_Called_DownloadSubtitle()
        {
            var filePath = mediaPaths.First();
            var directoryPath = Path.GetDirectoryName(filePath);
            var subtitle = _subtitleService.FindSubtitles(filePath, "eng").FirstOrDefault();
            _downloadSubtitleService.DownloadSubtitle(subtitle, directoryPath);
            Assert.That(File.Exists(String.Concat(directoryPath, Path.DirectorySeparatorChar, subtitle.SubtitleFileName)), Is.True);
        }

        [Test]
        public void Download_Subtitle_And_Rename_To_Given_Directory_When_Called_DownloadSubtitle_With_Adjust()
        {
            var filePath = mediaPaths.First();
            var directoryPath = Path.GetDirectoryName(filePath);
            var subtitle = _subtitleService.FindSubtitles(filePath, "eng").FirstOrDefault();
            _downloadSubtitleService.DownloadSubtitle(subtitle, directoryPath, filePath);
            Assert.That(File.Exists(String.Concat(directoryPath, Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(filePath), ".srt")), Is.True);
        }

        [Test]
        public void Download_Subtitles_To_Given_Directory_When_Called_DownloadSubtitle()
        {
            var filePath = mediaPaths.First();
            var directoryPath = Path.GetDirectoryName(filePath);
            var subtitles = _subtitleService.FindSubtitles(filePath, "eng");
            _downloadSubtitleService.DownloadSubtitles(subtitles, directoryPath);
            foreach (var subtitle in subtitles)
            {
                Assert.That(File.Exists(String.Concat(directoryPath, Path.DirectorySeparatorChar, subtitle.SubtitleFileName)), Is.True);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throw_ArgumentNullException_When_Called_DownloadSubtitle_With_Null_Subtitle()
        {
            var filePath = mediaPaths.First();
            var directoryPath = Path.GetDirectoryName(filePath);
            _downloadSubtitleService.DownloadSubtitle(null, directoryPath);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throw_ArgumentNullException_When_Called_DownloadSubtitle_With_Null_DirectoryPath()
        {
            _downloadSubtitleService.DownloadSubtitle(new Subtitle(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throw_ArgumentNullException_When_Called_DownloadSubtitles_With_Null_Subtitles()
        {
            var filePath = mediaPaths.First();
            var directoryPath = Path.GetDirectoryName(filePath);
            _downloadSubtitleService.DownloadSubtitles(null, directoryPath);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throw_ArgumentNullException_When_Called_DownloadSubtitles_With_Null_DirectoryPath()
        {
            _downloadSubtitleService.DownloadSubtitles(new List<Subtitle> { new Subtitle() }, null);
        }
    }
}