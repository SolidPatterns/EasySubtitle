using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySubtitle.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var progressDialog = new ProgressDialogWindow();
            var progressDialogViewModel = new ProgressDialogViewModel();

            var workList = Enumerable.Range(0, 999).ToArray();
            progressDialogViewModel.Progress = 0;
            progressDialogViewModel.ProgressMax = workList.Length;

            progressDialog.DataContext = progressDialogViewModel;
            progressDialog.Show();


            Task.Factory.StartNew(() =>
            {
                foreach (var i in workList)
                {
                    Thread.Sleep(300);
                    progressDialogViewModel.IncrementProgressCounter(10);
                }
            });
        }
    }
}
