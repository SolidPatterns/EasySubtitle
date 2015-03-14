using System.ComponentModel;
using System.Windows;

namespace EasySubtitle.WPF.Windows
{
    /// <summary>
    /// Interaction logic for ProgressDialogWindow.xaml
    /// </summary>
    public partial class ProgressDialogWindow : Window
    {
        public ProgressDialogWindow()
        {
            InitializeComponent();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            var viewModel = (ProgressDialogViewModel)this.DataContext;
            viewModel.Cancel.Execute(null);
        }
    }
}
