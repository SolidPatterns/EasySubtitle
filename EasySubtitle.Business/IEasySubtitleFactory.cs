namespace EasySubtitle.Business
{
    public interface IEasySubtitleFactory
    {
        ISubtitleService GetSubtitleService(ISubtitleClientCredentials credentials = null);
    }
}