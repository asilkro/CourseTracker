using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Dispatching;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Data;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;


namespace CourseTracker.Maui.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public TermsDB termsDB;
        public CourseDB courseDB;
        public AssessmentDB assessmentDB;

        protected IDispatcher Dispatcher => DispatcherProvider.Current.GetForCurrentThread();
        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModelBase()
        {
            termsDB = new TermsDB();
            courseDB = new CourseDB();
            assessmentDB = new AssessmentDB();
        }
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void InvokeOnMainThread(Action action)
        {
            if (Dispatcher.IsDispatchRequired)
            {
                Dispatcher.Dispatch(action);
            }
            else
            {
                action.Invoke();
            }
        }

        private DateTime minimumDate = DateTime.Parse("01/01/2020");
        public DateTime MinimumDate
        {
            get { return minimumDate; }
            set
            {
                if (minimumDate != value)
                {
                    minimumDate = value;
                    OnPropertyChanged(nameof(MinimumDate));
                }
            }
        }

        private DateTime maximumDate = DateTime.Parse("12/31/4020");
        public DateTime MaximumDate
        {
            get { return maximumDate; }
            set
            {
                if (maximumDate != value)
                {
                    maximumDate = value;
                    OnPropertyChanged(nameof(MaximumDate));
                }
            }
        }


        public async void ShowToast(string message)
        {
            CancellationTokenSource cancellationTokenSource = new();

            string text = message;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        public async Task CancelButtonClicked() //Used by all cancel buttons
        {
            await Shell.Current.GoToAsync("//homepage");
        }

        public DateTime dateStart = DateTime.Now.Date;
        public DateTime dateEnd = DateTime.Now.Date.AddDays(90);

    }
}
