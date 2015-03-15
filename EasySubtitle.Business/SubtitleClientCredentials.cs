namespace EasySubtitle.Business
{
    public class SubtitleClientCredentials
    {
        public string UserAgent { get; set; }

        public static SubtitleClientCredentials Default()
        {
            return new SubtitleClientCredentials
            {
                UserAgent = "OSTestUserAgent"
            };
        }
    }
}
