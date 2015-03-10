using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySubtitle.WPF
{
    public class ProgressDialogViewModel : ViewModelBase
    {
        #region Fields

        // Property variables
        private int p_Progress;
        private string p_ProgressMessage;
        private int p_ProgressMax;

        //Member variables
        private string m_ProgressMessageTemplate;
        private string m_CancellationMessage;

        #endregion

        #region Constructor

        public ProgressDialogViewModel()
        {
            Initialize();
        }

        #endregion

        #region Admin Properties

        /// <summary>
        /// A cancellation token source for the background operations.
        /// </summary>
        internal CancellationTokenSource TokenSource { get; set; }

        /// <summary>
        /// Whether the operation in progress has been cancelled.
        /// </summary>
        /// <remarks> 
        /// The Cancel command is invoked by the Cancel button, and on the window
        /// close (in case the user clicks the close box to cancel. The Cancel 
        /// command sets this property and checks it to make sure that the command 
        /// isn't run twice when the user clicks the Cancel button (once for the 
        /// button-click, and once for the window-close.
        /// </remarks>
        public bool IsCancelled { get; set; }
        #endregion

        #region Command Properties

        /// <summary>
        /// The Cancel command.
        /// </summary>
        public ICommand Cancel { get; set; }

        #endregion

        #region Data Properties

        /// <summary>
        /// The progress of an image processing job.
        /// </summary>
        /// <remarks>
        /// The setter for this property also sets the ProgressMessage property.
        /// </remarks>
        public int Progress
        {
            get { return p_Progress; }

            set
            {
                base.RaisePropertyChangingEvent("Progress");
                p_Progress = value;
                base.RaisePropertyChangedEvent("Progress");
            }
        }

        /// <summary>
        /// The maximum progress value.
        /// </summary>
        /// <remarks>
        /// The 
        /// </remarks>
        public int ProgressMax
        {
            get { return p_ProgressMax; }

            set
            {
                base.RaisePropertyChangingEvent("ProgressMax");
                p_ProgressMax = value;
                base.RaisePropertyChangedEvent("ProgressMax");
            }
        }

        /// <summary>
        /// The status message to be displayed in the View.
        /// </summary>
        public string ProgressMessage
        {
            get { return p_ProgressMessage; }

            set
            {
                base.RaisePropertyChangingEvent("ProgressMessage");
                p_ProgressMessage = value;
                base.RaisePropertyChangedEvent("ProgressMessage");
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Clears the view model.
        /// </summary>
        internal void ClearViewModel()
        {
            p_Progress = 0;
            p_ProgressMax = 0;
            p_ProgressMessage = "Preparing to perform simulated work.";
            this.IsCancelled = false;
        }

        /// <summary>
        /// Advances the progress counter for the Progress dialog.
        /// </summary>
        /// <param name="incrementClicks">The number of 'clicks' to advance the counter.</param>
        public void IncrementProgressCounter(int incrementClicks)
        {
            // Increment counter
            this.Progress += incrementClicks;

            // Update progress message
            var progress = Convert.ToSingle(p_Progress);
            var progressMax = Convert.ToSingle(p_ProgressMax);
            var f = (progress / progressMax) * 100;
            var percentComplete = Single.IsNaN(f) ? 0 : Convert.ToInt32(f);
            this.ProgressMessage = string.Format(m_ProgressMessageTemplate, percentComplete);
        }

        /// <summary>
        /// Sets the progreess message to show that processing was cancelled.
        /// </summary>
        internal void ShowCancellationMessage()
        {
            this.ProgressMessage = m_CancellationMessage;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        /// <param name="mainWindowViewModel">The view model for this application's main window.</param>
        private void Initialize()
        {
            m_ProgressMessageTemplate = "Simulated work {0}% complete";
            m_CancellationMessage = "Simulated work cancelled";
            this.ClearViewModel();
            TokenSource = new CancellationTokenSource();
            this.Cancel = new CancelCommand(this);

            var workList = Enumerable.Range(0, 999).ToArray();
            Progress = 0;
            ProgressMax = workList.Length;

            var task = Task.Factory.StartNew(() =>
            {
                foreach (var i in workList)
                {
                    if (TokenSource.IsCancellationRequested)
                    {
                        ShowCancellationMessage();
                        break;
                    }

                    Thread.Sleep(300);
                    IncrementProgressCounter(10);
                }
            }, TokenSource.Token);
        }

        #endregion
    }

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

    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
                _action.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }

    public class SelectedFile
    {
        public SelectedFile()
        {

        }

        public SelectedFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("filePath");

            File = filePath;
            Checked = true;
            Subtitles = new List<FoundSubtitle>();

            try
            {
                FileName = Path.GetFileName(filePath);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("File path is not correct.");
            }
        }

        public string File { get; set; }
        public string FileName { get; set; }
        public bool Checked { get; set; }
        public IList<FoundSubtitle> Subtitles { get; set; }

        public void CheckSubtitles()
        {
            var random = new Random();
            Subtitles.Add(new FoundSubtitle
            {
                Checked = true,
                SubtitleName = String.Format("subtitle {0}.srt", random.Next(0, 20))
            });

            Subtitles.Add(new FoundSubtitle
            {
                Checked = false,
                SubtitleName = String.Format("subtitle {0}.srt", random.Next(0, 20))
            });
        }
    }

    public class FoundSubtitle
    {
        public string SubtitleName { get; set; }
        public bool Checked { get; set; }
    }
}