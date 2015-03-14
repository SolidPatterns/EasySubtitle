using System;
using System.IO;
using EasySubtitle.WPF;
using EasySubtitle.WPF.ViewModels;
using EasySubtitle.WPF.Windows;

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
