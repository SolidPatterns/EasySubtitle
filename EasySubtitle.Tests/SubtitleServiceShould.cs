using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EasySubtitle.Business;
using NUnit.Framework;
using OSDBnet;

namespace EasySubtitle.Tests
{
    [TestFixture]
    public class SubtitleServiceShould
    {
        private ISubtitleService _subtitleService;
        private readonly List<string> mediaPaths = new List<string>
        {
            "F:/Videos/Series/Supernatural/Supernatural.S10E11.1080p.HDTV.X264-DIMENSION.mkv",
            "F:/Videos/Series/Supernatural/Supernatural.S10E13.1080p.HDTV.X264-DIMENSION.mkv",
            "F:/Videos/Series/Supernatural/Supernatural.S10E14.1080p.HDTV.X264-DIMENSION.mkv"
        };

        [SetUp]
        public void SetUp()
        {
            _subtitleService = EasySubtitleFactory.Instance.GetSubtitleService();
        }

        [Test]
        public void Return_Subtitles_For_Multiple_Files_And_Languages()
        {
            var subtitles = _subtitleService.FindSubtitles(mediaPaths, "tur", "eng");
            Assert.That(subtitles, Is.Not.Null);
            Assert.That(subtitles.Any(), Is.True);
        }

        [Test]
        public async void Return_Subtitles_For_Multiple_Files_In_Parallel()
        {
            var subtitles = new List<Subtitle>();

            var task = Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(mediaPaths, (path, state, count) =>
                {
                    Debug.WriteLine("Finding subtitles for {0}", args: path);
                    Debug.WriteLine("Count: {0}", args: count);
                    using (var client = SubtitleClientFactory.GetSubtitleClient())
                    {
                        subtitles.AddRange(_subtitleService.FindSubtitles(client, path, "eng"));
                    }
                });
            });
            await task;

            Assert.That(subtitles, Is.Not.Null);
            Assert.That(subtitles.Any(), Is.True);
        }

        [Test]
        public void Return_Subtitles_For_One_File_And_One_Language()
        {
            var subtitles = _subtitleService.FindSubtitles(mediaPaths.First(), "tur");
            Assert.That(subtitles, Is.Not.Null);
            Assert.That(subtitles.Any(), Is.True);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throw_Arg_Null_Exp_When_Called_With_Null_File_Name()
        {
            _subtitleService.FindSubtitles("", "tur");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throw_Arg_Null_Exp_When_Called_With_Null_Language_Name()
        {
            _subtitleService.FindSubtitles(mediaPaths.First(), "");
        }
    }
}