using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasySubtitle.WPF;

namespace EasySubtitle.Console
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var app = new App();

            //RunProgressDialog(app);

            var advancedSearchSubtitles = new AdvancedSubtitlesWindow
            {
                DataContext = new SearchAdvancedSubtitleViewModel(Directory.GetFiles("F:/Videos/Series/Arrow/S3", "*.mkv"))
            };
            app.Run(advancedSearchSubtitles);

            System.Console.ReadLine();
        }

        private static void RunProgressDialog(App app)
        {
            var progressDialog = new ProgressDialogWindow();
            var progressDialogViewModel = new ProgressDialogViewModel();
            progressDialog.DataContext = progressDialogViewModel;
            app.Run(progressDialog);
        }
    }
}
