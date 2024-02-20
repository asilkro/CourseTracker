using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui.ViewModels
{
    public class ListTermsVM : ViewModelBase
    {
        public List<Term> Terms { get; set; }

        public ListTermsVM()
        {
            Terms = [];
        }
        public bool IsRefreshing { get; set; }
        public async Task LoadTerms()
        {
            IsRefreshing = true;
            try
            {
                Terms ??= [];
                if (Terms.Count > 0)
                {
                    Terms = [];
                }
                List<Term> terms = await termsDB.GetTermsAsync();
                try
                {
                    int i = 0;
                    foreach (Term term in terms)
                    {
                        Terms.Add(term);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    ShowToast("Issue loading terms: " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                ShowToast("Issue loading terms: " + ex.Message);
            }
            finally
            {
                IsRefreshing = false;
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

        private string? termName;
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

        public async void ShowActionSheet(Term term)
        {
            string action = await App.Current.MainPage.DisplayActionSheet("Term Actions", "Cancel", null, "Edit Term", "Delete Term");
            switch (action)
            {
                case "Edit Term":
                    await Shell.Current.GoToAsync($"{nameof(TermPage)}?{nameof(TermVM.EditTermId)}={term.TermId}");
                    break;
                case "Delete Term":
                    await sharedDB.DeleteTermAndRelatedEntities(term);
                    await LoadTerms();
                    await Shell.Current.GoToAsync("..");
                    break;
                default:
                    break;
            }
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Term selectedTerm)
            {
                ShowActionSheet(selectedTerm);
            }
        ((ListView)sender).SelectedItem = null;
        }

        public async void OnAppearing()
        {
            await LoadTerms();
        }
    }
}
