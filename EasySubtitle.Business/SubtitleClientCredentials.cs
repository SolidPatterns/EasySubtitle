namespace EasySubtitle.Business
{
    public class SubtitleClientCredentials : ISubtitleClientCredentials
    {
        private static IEasySubtitleConfig _easySubtitleConfig;

        public SubtitleClientCredentials(IEasySubtitleConfig easySubtitleConfig)
        {
            _easySubtitleConfig = easySubtitleConfig;
        }

        public SubtitleClientCredentials()
        {
            _easySubtitleConfig = EasySubtitleConfig.GetEasySubtitleConfig();
        }

        public string UserAgent { get; set; }

        public static ISubtitleClientCredentials Default()
        {
            return new SubtitleClientCredentials
            {
                UserAgent = GetUserAgent()
            };
        }

        private static string GetUserAgent()
        {
            var userAgent = _easySubtitleConfig.UserAgent;
            if (string.IsNullOrWhiteSpace(userAgent))
                userAgent = "OSTestUserAgent";
            return userAgent;
        }
    }
}