using System.Configuration;
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
        public void Change_User_Agent_When_Set_Value()
        {
            const string userAgent = "kemal";
            Config.UserAgent = userAgent;
            Assert.AreEqual(userAgent, ConfigurationManager.AppSettings.Get(EasySubtitleConfig.SubtitleClientUserAgentKey));
        }
    }
}