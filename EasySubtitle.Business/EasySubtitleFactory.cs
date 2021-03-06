using System;

namespace EasySubtitle.Business
{
    public class EasySubtitleFactory : IEasySubtitleFactory
    {
        private static volatile Lazy<IEasySubtitleFactory> _instance = new Lazy<IEasySubtitleFactory>(() => new EasySubtitleFactory());

        protected EasySubtitleFactory()
        {
        }

        public static IEasySubtitleFactory Instance
        {
            get { return _instance.Value; }
        }

        public ISubtitleService GetSubtitleService(ISubtitleClientCredentials credentials = null)
        {
            if (credentials == null)
                credentials = SubtitleClientCredentials.Default();
            return new SubtitleService(credentials);
        }
    }
}