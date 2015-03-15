namespace EasySubtitle.Business
{
    public interface IEasySubtitleFactory
    {
        ISubtitleService GetSubtitleService(SubtitleClientCredentials credentials = null);
        IDownloadSubtitleService GetDownloadSubtitleService(SubtitleClientCredentials credentials = null);
    }
}