using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.ViewModels
{
    internal class AddTermsVM : ViewModelBase
    {
        private Term term = new();

        public Term Term
        {
            get { return term; }
            set
            {
                if (term != value)
                {
                    term = value;
                    OnPropertyChanged("Term");
                }
            }
        }
    }
}
