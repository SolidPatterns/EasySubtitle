using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasySubtitle.Business;
using EasySubtitle.WPF.ViewModels;
using EasySubtitle.WPF.Windows;
using OSDBnet;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace EasySubtitle.ShellExtension
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class FindSubtitlesExtension : SharpContextMenu
    {
        private readonly IEasySubtitleConfig _config;
        private const String Source = "EasySubtitle.ShellExtension";
        private const String LogType = "Application";
        private const String Event = "Error";

        public FindSubtitlesExtension()
        {
            if (!EventLog.SourceExists(Source))
                EventLog.CreateEventSource(Source, LogType);

            try
            {
                _config = RegistryConfig.Instance;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(Source, ex.Message, EventLogEntryType.Error);
                throw;
            }
        }
        
        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            //  Create the menu strip.
            var menu = new ContextMenuStrip();

            //  Create a 'easySubtitleMenu' item.
            var easySubtitleMenu = new ToolStripMenuItem
            {
                Text = "Easy Subtitle",
            };

            var findSubtitlesMenuItem = GetFindSubtitlesMenuItem();
            var findSubtitlesAdvancedMenuItem = GetFindSubtitlesAdvancedMenuItem();

            easySubtitleMenu.DropDownItems.Add(findSubtitlesMenuItem);
            easySubtitleMenu.DropDownItems.Add(findSubtitlesAdvancedMenuItem);

            //  Add the item to the context menu.
            menu.Items.Add(easySubtitleMenu);

            //  Return the menu.
            return menu;
        }

        private ToolStripMenuItem GetFindSubtitlesAdvancedMenuItem()
        {
            var findSubtitlesAdvancedMenuItem = new ToolStripMenuItem
            {
                Text = "Find Subtitle(s) (Advanced)"
            };
            findSubtitlesAdvancedMenuItem.Click += (sender, args) => FindSubtitlesAdvanced();
            return findSubtitlesAdvancedMenuItem;
        }

        private ToolStripMenuItem GetFindSubtitlesMenuItem()
        {
            var findSubtitlesMenuItem = new ToolStripMenuItem
            {
                Text = "Find Subtitle(s)"
            };

            //  When we click, we'll call the 'FindSubtitles' function.
            findSubtitlesMenuItem.Click += (sender, args) => FindSubtitles();
            return findSubtitlesMenuItem;
        }

        private async void FindSubtitles()
        {
            //var app = new App();
            //app.Run(new Progress());

            var subtitleService = GetSubtitleService();
            IAnonymousClient[] client = {EasySubtitleClientFactory.GetSubtitleClient()};

            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    IAnonymousClient anonymousClient = client[0];
                    Parallel.ForEach(SelectedItemPaths, (path, state, count) =>
                    {
                        Debug.WriteLine("Finding subtitles for {0}", args: path);
                        Debug.WriteLine("Count: {0}", args: count);
                        var subtitle = subtitleService.FindSubtitles(anonymousClient, path, _config.DefaultSubtitleLanguage).FirstOrDefault();
                        if (subtitle == null)
                            return;
                        subtitleService.DownloadSubtitleAdjusted(anonymousClient, subtitle, path);
                    });
                });

                await task;
                MessageBox.Show("Finding subtitles completed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("Error occured. Details: {0}.", e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                client[0].Dispose();
                client[0] = null;
            }
        }

        private ISubtitleService GetSubtitleService()
        {
            var subtitleService = EasySubtitleFactory.Instance.GetSubtitleService();
            return subtitleService;
        }

        private void FindSubtitlesAdvanced()
        {
            var advancedSearchSubtitles = new AdvancedSubtitlesWindow
            {
                DataContext = new SearchAdvancedSubtitleViewModel(SelectedItemPaths, EasySubtitleFactory.Instance.GetSubtitleService())
            };
            advancedSearchSubtitles.Show();
        }
    }
}
