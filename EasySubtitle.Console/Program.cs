using System;
using System.Collections.Generic;
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

            var progressDialog = new ProgressDialogWindow();
            var progressDialogViewModel = new ProgressDialogViewModel();

            //var workList = Enumerable.Range(0, 999).ToArray();
            //progressDialogViewModel.Progress = 0;
            //progressDialogViewModel.ProgressMax = workList.Length;

            progressDialog.DataContext = progressDialogViewModel;
            

            app.Run(progressDialog);
            //progressDialog.Show();

            System.Console.ReadLine();
        }
    }
}
