using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.ViewModels
{
    internal class ListTermsVM : ViewModelBase
    {
        private List<Term> terms;

        public List<Term> Terms
        {
            get { return terms; }
            set
            {
                if (terms != value)
                {
                    terms = value;
                    OnPropertyChanged("Terms");
                }
            }
        }

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
