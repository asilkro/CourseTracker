using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Factories;

namespace CourseTracker.Maui.Views;

public partial class ListTerms : ContentPage
{
	ListTermsVM viewModel;
    readonly Connection _database;
    readonly TermFactory _termFactory;

    public ListTerms(TermFactory termFactory)
    {
        InitializeComponent();
		viewModel = new ListTermsVM();
		BindingContext = viewModel;
        _termFactory = termFactory;
        if (_database == null)
        {
            _database = new Connection();
            _database.GetAsyncConnection();
        }
        var list = _database.Table<Term>();
        termListView.ItemsSource = (System.Collections.IEnumerable)list;
	}

    private async Task ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		if (e.Item is Term selectedTerm)
		{
            // Navigate to EditTerm view, passing the selectedTerm
            await Navigation.PushAsync(new EditTerms(_termFactory,selectedTerm));
        }

            // Deselect the item
            ((ListView)sender).SelectedItem = null;
    }
 
}