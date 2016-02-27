using System.Collections.Generic;
using EasySubtitle.Business;
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
            Config = RegistryConfig.GetEasySubtitleConfig();
        }

        [TearDown]
        public void TearDown()
        {
            Config.ResetToDefaults();
        }

        [Test]
        public void Change_Default_Subtitile_Language_When_Set_Value()
        {
            const string newDefaultSubtitileLanguage = "tr";
            Config.DefaultSubtitleLanguage = newDefaultSubtitileLanguage;
            Assert.AreEqual(newDefaultSubtitileLanguage, Config.DefaultSubtitleLanguage);
        }

        [Test]
        public void Change_Selected_Subtitile_Languages_When_Set_Value()
        {
            const string expectedNewSelectedSubtitileLanguage = "tr,en";
            var subtitleList = new List<string>() {"tr", "en"};
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