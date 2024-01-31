using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CourseTracker.Maui.ViewModels
{
    public class HomepageVM : INotifyPropertyChanged
    {
        private double _sliderValue;
        private string _labelText;

        public double SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                LabelText = ((int)value).ToString();
                OnPropertyChanged();
            }
        }

        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
