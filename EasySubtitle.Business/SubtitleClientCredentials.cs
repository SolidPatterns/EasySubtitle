using System.Configuration;

namespace EasySubtitle.Business
{
    public class SubtitleClientCredentials
    {
        public string UserAgent { get; set; }

        public static SubtitleClientCredentials Default()
        {
            return new SubtitleClientCredentials
            {
                UserAgent = GetUserAgent()
            };
        }

        private static string GetUserAgent()
        {
            var userAgent = ConfigurationManager.AppSettings.Get("SubtitleClientUserAgent");
            if (string.IsNullOrWhiteSpace(userAgent))
                userAgent = "OSTestUserAgent";
            return userAgent;
        }
    }
}