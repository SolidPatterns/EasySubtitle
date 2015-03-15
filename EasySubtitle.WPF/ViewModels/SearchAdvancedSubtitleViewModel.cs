using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EasySubtitle.Business;
using EasySubtitle.Business.Models;
using EasySubtitle.WPF.Commands;
using EasySubtitle.WPF.Models;
using EasySubtitle.WPF.Windows;
using OSDBnet;

namespace EasySubtitle.WPF.ViewModels
{
    public class SearchAdvancedSubtitleViewModel : ViewModelBase
    {
        private readonly ISubtitleService _subtitleService;
        private SelectedFile _selectedFile;
        private CancellationTokenSource _tokenSource;
        private ProgressDialogViewModel _dataContext;
        private ProgressDialogWindow _progress;

        public SearchAdvancedSubtitleViewModel(IEnumerable<string> selectedFilePaths, ISubtitleService subtitleService)
        {
            _subtitleService = subtitleService;
            if (selectedFilePaths == null || !selectedFilePaths.Any()) throw new ArgumentNullException("selectedFilePaths");

            PropertyChanged += SelectedFileChanged;

            SelectedFiles = new List<SelectedFile>();

            selectedFilePaths.ToList().ForEach(x => SelectedFiles.Add(new SelectedFile(x)));
            SelectedFile = SelectedFiles.FirstOrDefault();

            Download = new DelegateCommand(() =>
            {
                _tokenSource = new CancellationTokenSource();
                _dataContext = new ProgressDialogViewModel
                {
                    TokenSource = _tokenSource,
                    Progress = 0,
                    ProgressMax = SelectedFiles.Count
                };
                if (_progress == null)
                    _progress = new ProgressDialogWindow { DataContext = _dataContext };
                else
                    _progress.DataContext = _dataContext;
                _progress.Show();
                _progress.Focus();

                DownlaodSubtitles();
            });
        }

        private async void DownlaodSubtitles()
        {
            IAnonymousClient[] client = {SubtitleClientFactory.GetSubtitleClient()};
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    var taskClient = client[0];
                    Parallel.ForEach(SelectedFiles, (selectedFile, state, count) =>
                    {
                        Debug.WriteLine("Finding subtitles for {0}", args: selectedFile.File);
                        Debug.WriteLine("Count: {0}", args: count);

                        if (_tokenSource.IsCancellationRequested)
                        {
                            _dataContext.ShowCancellationMessage();
                            state.Stop();
                            return;
                        }

                        _dataContext.IncrementProgressCounter(1);

                        if (!selectedFile.Checked)
                        {
                            return;
                        }

                        if (selectedFile.Subtitles.Any() && selectedFile.Subtitles.Any(sub => sub.Checked))
                        {
                            var selectedSubtitles =
                                selectedFile.Subtitles.Where(sub => sub.Checked).Select(sub => sub.Subtitle);
                            _subtitleService.DownloadSubtitles(taskClient, selectedSubtitles, selectedFile.DirectoryPath);
                            return;
                        }

                        var subtitle = _subtitleService.FindSubtitles(taskClient, selectedFile.File, "eng").FirstOrDefault();
                        if (subtitle == null)
                        {
                            return;
                        }
                        _subtitleService.DownloadSubtitleAdjusted(taskClient, subtitle, selectedFile.File);
                    });
                }, _tokenSource.Token);
                await task;
            }
            catch (Exception)
            {
                MessageBox.Show("Error occurred.");
            }
            finally
            {
                client[0].Dispose();
                client[0] = null;
                _progress.Hide();
            }
        }

        public IList<SelectedFile> SelectedFiles { get; set; }

        public async void SelectedFileChanged(object obj, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("SelectedFile"))
                return;
            var file = SelectedFile;
            if (file.Subtitles.Any() || file.Searched)
                return;

            file.Searched = true;

            var subtitles = (await _subtitleService.FindSubtitlesAsync(file.File, "eng")).Select(sub => new FoundSubtitle { Subtitle = sub }).ToList();
            var firstSub = subtitles.FirstOrDefault();
            if (firstSub != null)
                firstSub.Checked = true;

            file.Subtitles = subtitles;

            base.RaisePropertyChangedEvent("SelectedFile");
        }

        public SelectedFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                base.RaisePropertyChangingEvent("SelectedFile");
                //SelectedFileChanged(value);
                _selectedFile = value;
                base.RaisePropertyChangedEvent("SelectedFile");
            }
        }

        public ICommand Download { get; set; }
    }
}