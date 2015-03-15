using System;
using System.Collections.Generic;
using System.Linq;
using EasySubtitle.Business;
using NUnit.Framework;

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