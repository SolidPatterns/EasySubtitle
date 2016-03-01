using System;
using System.Collections.Generic;
using EasySubtitle.Business;
using EasySubtitle.Business.Models;
using NUnit.Framework;

namespace EasySubtitle.Tests
{
    [TestFixture]
    public class EasySubtitleConfigShould
    {
        public IEasySubtitleConfig Config;

        [SetUp]
        public void SetUp()
        {
            Config = RegistryConfig.Instance;
        }

        [TearDown]
        public void TearDown()
        {
            Config.ResetToDefaults(AppDomain.CurrentDomain.BaseDirectory);
        }

        [Test]
        public void Change_Default_Subtitile_Language_When_Set_Value()
        {
            const string newDefaultSubtitileLanguage = SubtitleLanguages.Turkish;
            Config.DefaultSubtitleLanguage = newDefaultSubtitileLanguage;
            Assert.AreEqual(newDefaultSubtitileLanguage, Config.DefaultSubtitleLanguage);
        }

        [Test]
        public void Change_Selected_Subtitile_Languages_When_Set_Value()
        {
            var expectedNewSelectedSubtitileLanguage = String.Join(",", SubtitleLanguages.Turkish, SubtitleLanguages.English);
            var subtitleList = new List<string>() { SubtitleLanguages.Turkish, SubtitleLanguages.English };
            Config.SelectedSubtitleLanguages = subtitleList;
            Assert.AreEqual(expectedNewSelectedSubtitileLanguage, string.Join(",",Config.SelectedSubtitleLanguages));
        }

        [Test]
        public void Change_UserAgent_When_Set_Value()
        {
            const string newUserAgent = "some user agent";
            Config.UserAgent = newUserAgent;
            Assert.AreEqual(newUserAgent, Config.UserAgent);
        }
    }
}