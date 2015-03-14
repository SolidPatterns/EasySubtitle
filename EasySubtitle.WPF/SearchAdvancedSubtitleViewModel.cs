using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace EasySubtitle.WPF
{
    public class SearchAdvancedSubtitleViewModel : ViewModelBase
    {
        private SelectedFile _selectedFile;

        public SearchAdvancedSubtitleViewModel(IEnumerable<string> selectedFilePaths)
        {
            if (selectedFilePaths == null || !selectedFilePaths.Any()) throw new ArgumentNullException("selectedFilePaths");

            SelectedFiles = new List<SelectedFile>();

            selectedFilePaths.ToList().ForEach(x => SelectedFiles.Add(new SelectedFile(x)));
            SelectedFile = SelectedFiles.FirstOrDefault();

            Download = new DelegateCommand(() =>
            {
                DownlaodSubtitles();
                var model = new ProgressDialogViewModel();
                var progress = new ProgressDialogWindow {DataContext = model};
                progress.Show();
            });
        }

        private void DownlaodSubtitles()
        {
            SelectedFiles.ToList().Where(x => !x.Subtitles.Any()).ToList().ForEach(x => x.CheckSubtitles());

            //download.. call download service etc.
        }

        public IList<SelectedFile> SelectedFiles { get; set; }

        public SelectedFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                base.RaisePropertyChangingEvent("SelectedFile");
                _selectedFile = value;
                _selectedFile.CheckSubtitles();
                base.RaisePropertyChangedEvent("SelectedFile");
            }
        }

        public ICommand Download { get; set; }
    }
}