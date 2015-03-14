namespace EasySubtitle.Business
{
    public class SubtitleServiceFactory
    {
        public static ISubtitleService GetSubtitleService(SubtitleServiceCredentials credentials)
        {
            return new SubtitleService(credentials);
        }
    }
}