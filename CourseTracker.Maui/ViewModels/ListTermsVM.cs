using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.ViewModels
{
    public class ListTermsVM : ViewModelBase
    {
        public ObservableCollection<Term> Terms { get; private set; } = new ObservableCollection<Term>();

        private Connection _database;
        public ListTermsVM()
        {
            LoadTerms();
        }

        public async Task LoadTerms()
        {
            try
            {
                _database = _database ?? new Connection();
                var updatedTermsList = await _database.Table<Term>();
                Terms.Clear();
                foreach (var term in updatedTermsList)
                {
                    Terms.Add(term);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Issue loading terms: " + ex.Message);
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
