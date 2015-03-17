using System;
using System.IO;
using EasySubtitle.Business;
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
                DataContext = new SearchAdvancedSubtitleViewModel(Directory.GetFiles("F:/Videos/Series/Arrow/S3", "*.mkv"), EasySubtitleFactory.Instance.GetSubtitleService())
            };
            app.Run(advancedSearchSubtitles);

            //var settingsViewModel = new SettingsViewModel();
            //var settingsWindow = new SettingsWindow();
            //settingsWindow.DataContext = settingsViewModel;
            //app.Run(settingsWindow);

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
