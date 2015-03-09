using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EasySubtitle.WPF
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

    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Administrative Properties

        /// <summary>
        /// Whether the view model should ignore property-change events.
        /// </summary>
        public virtual bool IgnorePropertyChangeEvents { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        public virtual void RaisePropertyChangedEvent(string propertyName)
        {
            // Exit if changes ignored
            if (IgnorePropertyChangeEvents) return;

            // Exit if no subscribers
            if (PropertyChanged == null) return;

            // Raise event
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, e);
        }

        /// <summary>
        /// Raises the PropertyChanging event.
        /// </summary>
        /// <param name="propertyName">The name of the changing property.</param>
        public virtual void RaisePropertyChangingEvent(string propertyName)
        {
            // Exit if changes ignored
            if (IgnorePropertyChangeEvents) return;

            // Exit if no subscribers
            if (PropertyChanging == null) return;

            // Raise event
            var e = new PropertyChangingEventArgs(propertyName);
            PropertyChanging(this, e);
        }

        #endregion
    }

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

    public class CancelCommand : ICommand
    {
        #region Fields

        // Member variables
        private ProgressDialogViewModel m_ViewModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CancelCommand(ProgressDialogViewModel viewModel)
        {
            m_ViewModel = viewModel;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether the CancelCommand is enabled.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Actions to take when CanExecute() changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the CancelCommand
        /// </summary>
        public void Execute(object parameter)
        {
            /* The Cancel command is invoked by the Cancel button, and on the window
             * close (in case the user clicks the close box to cancel. The Cancel 
             * command sets this property and checks it to make sure that the command 
             * isn't run twice when the user clicks the Cancel button (once for the 
             * button-click, and once for the window-close. */

            // Exit if dialog has already been cancelled
            if (m_ViewModel.IsCancelled) return;

            /* The DoDemoWorkCommand.Execute() method defines a cancellation token source and
             * passes it to the Progress Dialog view model. The token itself is passed to the 
             * parallel image processing loop defined in the GoCommand.DoWork()  method. We 
             * cancel the loop by calling the TokenSource.Cancel() method. */

            // Validate TokenSource object
            if (m_ViewModel.TokenSource == null)
            {
                throw new ApplicationException("ProgressDialogViewModel.TokenSource property is null");
            }

            // Cancel all pending background tasks
            m_ViewModel.TokenSource.Cancel();
        }

        #endregion
    }
}
