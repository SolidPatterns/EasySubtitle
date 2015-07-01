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
            Config = EasySubtitleConfig.GetEasySubtitleConfig();
        }

        [Test]
        public void Change_Default_Subtitile_Language_When_Set_Value()
        {
            const string newDefaultSubtitileLanguage = "tur";
            Config.SetDefaultSubtitileLanguage(newDefaultSubtitileLanguage);
            Assert.AreEqual(newDefaultSubtitileLanguage, Config.DefaultSubtitleLanguage);
        }

        [Test]
        public void Change_Selected_Subtitile_Languages_When_Set_Value()
        {
            const string expectedNewSelectedSubtitileLanguage = "tur,eng";
            var subtitleList = new List<string>() {"tur", "eng"};
            Config.SetSelectedSubtitleLanguages(subtitleList);
            Assert.AreEqual(expectedNewSelectedSubtitileLanguage, string.Join(",",Config.SelectedSubtitleLanguages));
        }
    }
}