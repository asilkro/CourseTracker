using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
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
                if (Terms == null)
                {
                    Terms = new List<Term>();
                }
                if (Terms.Count > 0)
                {
                    Terms.Clear();
                }
                var terms = await termsDB.GetTermsAsync();
                Debug.WriteLine("Term count: "+ terms.Count);
                foreach (var term in terms)
                {
                    Terms.Add(term);
                }
                //Terms = new ObservableCollection<Term>(terms);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Issue loading terms: " + ex.Message);
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
                    //await NavigateToEditTermASync(term);
                    await Shell.Current.GoToAsync($"{nameof(TermPage)}?{nameof(TermVM.EditTermId)}={term.TermId}");
                    break;
                case "Delete Term":
                    await RemoveTermAsync(term);
                    break;
                default:
                    break;
            }
        }

        private async Task NavigateToEditTermASync(Term term) // workaround for not being able to use await Nav
        {
            //var TermId = term.TermId;
            //await App.Current.MainPage.Navigation.PushAsync(new TermPage(term));
            //await Shell.Current.GoToAsync($"{nameof(TermPage)}?{nameof(TermPage.TermId)}={term.TermId}");
        }

        private async Task RemoveTermAsync(Term term)
        {
            var result = await App.Current.MainPage.DisplayAlert("Delete Term", $"Are you sure you want to delete {term.TermName}?", "Yes", "No");
            if (result)
            {
                int confirm = await termsDB.DeleteTermAsync(term);
                if (confirm == 1)
                {
                    await App.Current.MainPage.DisplayAlert("Term Deleted Successfully", "", "Ok");
                }
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
