using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace CourseTracker.Maui.Views;

public partial class ListTerms : ContentPage
{
	private ListTermsVM viewModel;
    private Connection _database;
    private ObservableCollection<Term> _terms = new();

    public ListTerms()
    {
        InitializeComponent();
        BindingContext = new ListTermsVM();
    }

    private async Task InitializeDataAsync()
    {
        if (_database == null)
        {
            _database = new Connection();
            _database.GetAsyncConnection();
        }

        var list = await _database.Table<Term>();
        termListView.ItemsSource = list;
    }


    private void TermListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		if (e.Item is Term selectedTerm)
		{
            ShowActionSheet(selectedTerm);
        }

        ((ListView)sender).SelectedItem = null;
    }
    private async void ShowActionSheet(Term term)
    {
        string action = await DisplayActionSheet("Term Actions", "Cancel", null, "Edit Term", "Delete Term");
        switch (action)
        {
            case "Edit Term":
                await NavigateToEditTermASync(term);
                break;
            case "Delete Term":
                await RemoveTermAsync(term);
                break;
            default:
                break;
        }
    }

    private async Task RemoveTermAsync(Term term)
    {
        if (_database == null)
        {
            _database = new Connection();
            _database.GetAsyncConnection();
        }

        var result = await DisplayAlert("Delete Term", $"Are you sure you want to delete {term.TermName}?", "Yes", "No");
        if (result)
        {
            await _database.DeleteAsync(term);
            await LoadTerms();
        }
    }

    private async Task NavigateToEditTermASync(Term term) // workaround for not being able to use await Nav
    {
        await Navigation.PushAsync(new TermPage(term));
    }

    private async Task LoadTerms()
    {
        try
        {
            _database = _database ?? new Connection();
            var updatedTermsList = await _database.Table<Term>();
            _terms.Clear();
            foreach (var term in updatedTermsList)
            {
                _terms.Add(term);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Issue loading terms: " + ex.Message);
            return;
        }
    }

}