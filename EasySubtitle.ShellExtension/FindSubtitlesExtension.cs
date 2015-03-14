using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasySubtitle.Business;
using EasySubtitle.WPF;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace EasySubtitle.ShellExtension
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class FindSubtitlesExtension : SharpContextMenu
    {
        private readonly string[] _languages = { "tur" };
        //public void Test()
        //{
        //    IAnonymousClient client = Osdb.Login("OSTestUserAgent");

        //}

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

        private void FindSubtitles()
        {
            //var app = new App();
            //app.Run(new Progress());

            Task.Factory.StartNew(() =>
            {
                IList<Task> tasks = SelectedItemPaths.Select(path => Task.Factory.StartNew(() =>
                {
                    var subtitleService = GetSubtitleService();
                    subtitleService.FindSubtitles(path, _languages);
                })).ToList();

                Task.WaitAll(tasks.ToArray());

                //  Show the ouput.
                MessageBox.Show("Finding subtitles completed.");

            });
        }

        private ISubtitleService GetSubtitleService()
        {
            var subtitleService =
                SubtitleServiceFactory.GetSubtitleService(
                    new SubtitleServiceCredentials { UserAgent = "OSTestUserAgent" });
            return subtitleService;
        }

        private void FindSubtitlesAdvanced()
        {
            var app = new App();
            app.Run(new AdvancedSubtitlesWindow());
        }
    }
}
