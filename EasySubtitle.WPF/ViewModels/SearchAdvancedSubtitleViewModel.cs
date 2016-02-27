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
        private const string SelectedFilePropertyName = "SelectedFile";
        private readonly ISubtitleService _subtitleService;
        private SelectedFile _selectedFile;
        private CancellationTokenSource _tokenSource;
        private ProgressDialogViewModel _dataContext;
        private ProgressDialogWindow _progress;
        private readonly string[] _languages = { "en" };

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
            //todo: needs refactoring.
            //todo: appsettings.config is not at that path when registered as shell ext. fix that.
            IAnonymousClient[] client = { EasySubtitleClientFactory.GetSubtitleClient() };
            try
            {
                await DownloadSubtitlesAsync(client);
                MessageBox.Show("Finding subtitles completed.", "Done", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("Error occured. Details: {0}.", e.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
            finally
            {
                client[0].Dispose();
                client[0] = null;
                _progress.Hide();
            }
        }

        private Task DownloadSubtitlesAsync(IAnonymousClient[] client)
        {
            return Task.Factory.StartNew(() =>
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

                    if (!selectedFile.Checked)
                    {
                        IncrementProgressCounter();
                        return;
                    }

                    if (selectedFile.Searched && selectedFile.Subtitles.Any(sub => sub.Checked))
                    {
                        var selectedSubtitles =
                            selectedFile.Subtitles.Where(sub => sub.Checked).Select(sub => sub.Subtitle);
                        if (selectedSubtitles.Count() > 1)
                        {
                            _subtitleService.DownloadSubtitles(taskClient, selectedSubtitles, selectedFile.DirectoryPath);
                            IncrementProgressCounter();
                            return;
                        }
                            
                        _subtitleService.DownloadSubtitleAdjusted(taskClient, selectedSubtitles.First(), selectedFile.File);
                        IncrementProgressCounter();
                        return;
                    }

                    var subtitle = _subtitleService.FindSubtitles(taskClient, selectedFile.File, _languages).FirstOrDefault();
                    if (subtitle == null)
                    {
                        IncrementProgressCounter();

                        return;
                    }
                    _subtitleService.DownloadSubtitleAdjusted(taskClient, subtitle, selectedFile.File);
                    IncrementProgressCounter();

                });
            }, _tokenSource.Token);
        }

        private void IncrementProgressCounter()
        {
            _dataContext.IncrementProgressCounter(1);
        }

        public IList<SelectedFile> SelectedFiles { get; set; }

        public async void SelectedFileChanged(object obj, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals(SelectedFilePropertyName))
                return;
            var file = SelectedFile;
            if (file.Subtitles.Any() || file.Searched)
                return;

            file.Searched = true;

            var subtitles = (await _subtitleService.FindSubtitlesAsync(file.File, _languages)).Select(sub => new FoundSubtitle { Subtitle = sub }).ToList();
            var firstSub = subtitles.FirstOrDefault();
            if (firstSub != null)
                firstSub.Checked = true;

            file.Subtitles = subtitles;

            base.RaisePropertyChangedEvent(SelectedFilePropertyName);
        }

        public SelectedFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                base.RaisePropertyChangingEvent(SelectedFilePropertyName);
                _selectedFile = value;
                base.RaisePropertyChangedEvent(SelectedFilePropertyName);
            }
        }

        public ICommand Download { get; set; }
    }
}