using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Views;

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
                    OnPropertyChanged(nameof(Term));
                }
            }
        }

        private string termName;
        public string TermName
        {
            get { return termName; }
            set
            {
                if (termName != value)
                {
                    termName = value;
                    OnPropertyChanged(nameof(TermName));
                }
            }
        }

        private int termId;
        public int TermId
        {
            get { return termId; }
            set
            {
                if (termId != value)
                {
                    termId = value;
                    OnPropertyChanged(nameof(TermId));
                }
            }
        }

        private DateTime termStart;
        public DateTime TermStart
        {
            get { return termStart; }
            set
            {
                if (termStart != value)
                {
                    termStart = value;
                    OnPropertyChanged(nameof(TermStart));
                }
            }
        }

        private DateTime termEnd;
        public DateTime TermEnd
        {
            get { return termEnd; }
            set
            {
                if (termEnd != value)
                {
                    termEnd = value;
                    OnPropertyChanged(nameof(TermEnd));
                }
            }
        }

        private int courseCount;
        public int CourseCount
        {
            get { return courseCount; }
            set
            {
                if (courseCount != value)
                {
                    courseCount = value;
                    OnPropertyChanged(nameof(CourseCount));
                }
            }
        }


        private async Task NavigateToEditTermASync(Term term)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new TermPage(term));
        }

        private async Task RemoveTermAsync(Term term)
        {
            if (_database == null)
            {
                _database = new Connection();
                _database.GetAsyncConnection();
            }

            var result = await Application.Current.MainPage.DisplayAlert("Delete Term", $"Are you sure you want to delete {term.TermName}?", "Yes", "No");
            if (result)
            {
                await _database.DeleteAsync(term);
                await LoadTerms();
            }
        }

    }
}
