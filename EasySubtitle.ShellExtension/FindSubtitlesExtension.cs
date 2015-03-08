using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSDBnet;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace EasySubtitle.ShellExtension
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class FindSubtitlesExtension : SharpContextMenu
    {
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
            Task.Factory.StartNew(() =>
            {
                IList<Task> tasks = new List<Task>();

                using (IAnonymousClient client = Osdb.Login("OSTestUserAgent"))
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            var subtitles = client.SearchSubtitlesFromFile("tur", filePath);
                            if (!subtitles.Any())
                                return;
                            var subtitle = subtitles.OrderByDescending(x => x.Rating).FirstOrDefault();
                            if (subtitle != null)
                            {
                                var directoryPath = Path.GetDirectoryName(filePath);
                                client.DownloadSubtitleToPath(directoryPath, subtitle);
                                File.Move(String.Concat(directoryPath, Path.DirectorySeparatorChar.ToString(), subtitle.SubtitleFileName)
                                    , String.Concat(directoryPath, Path.DirectorySeparatorChar.ToString(), Path.GetFileNameWithoutExtension(filePath), ".srt"));
                            }
                        });
                        tasks.Add(task);
                    }
                    Task.WaitAll(tasks.ToArray());
                }

                //  Show the ouput.
                MessageBox.Show("Finding subtitles completed.");
            });
        }
    }
}
