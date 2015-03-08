using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OSDBnet;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace EasySubtitle.Core
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".mkv")]
    public class FindSubtitlesExtension : SharpContextMenu
    {
        public void Test()
        {
            IAnonymousClient client = Osdb.Login("OSTestUserAgent");
            
        }

        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            //  Create the menu strip.
            var menu = new ContextMenuStrip();

            //  Create a 'count lines' item.
            var itemCountLines = new ToolStripMenuItem
            {
                Text = "Find subtitle(s)",
                
            };

            //  When we click, we'll call the 'CountLines' function.
            itemCountLines.Click += (sender, args) => FindSubtitles();

            //  Add the item to the context menu.
            menu.Items.Add(itemCountLines);

            //  Return the menu.
            
            return menu;
        }

        private void FindSubtitles()
        {
            IAnonymousClient client = Osdb.Login("OSTestUserAgent");

            foreach (var filePath in SelectedItemPaths)
            {
                var subtitles = client.SearchSubtitlesFromFile("tur", filePath);
                var subtitle = subtitles.FirstOrDefault();
                if (subtitle != null)
                {
                    var directoryPath = Path.GetDirectoryName(filePath);
                    client.DownloadSubtitleToPath(directoryPath, subtitle);
                }
            }
        }
    }
}
