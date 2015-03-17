using OSDBnet;

namespace EasySubtitle.Business
{
    public class EasySubtitleClientFactory
    {
        public static IAnonymousClient GetSubtitleClient(SubtitleClientCredentials credentials = null)
        {
            if (credentials == null)
                credentials = SubtitleClientCredentials.Default();

            return Osdb.Login(credentials.UserAgent);
        }
    }
}