using OSDBnet;

namespace EasySubtitle.Business
{
    public class EasySubtitleClientFactory
    {
        public static IAnonymousClient GetSubtitleClient(ISubtitleClientCredentials credentials = null)
        {
            if (credentials == null)
                credentials = SubtitleClientCredentials.Default();

            return Osdb.Login(credentials.UserAgent);
        }
    }
}