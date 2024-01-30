using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Views;

public partial class ListTerms : ContentPage
{
	ListTermsVM viewModel;
    readonly Connection _database;

    public ListTerms()
    {
        InitializeComponent();
		viewModel = new ListTermsVM();
		BindingContext = viewModel;
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
            await Navigation.PushAsync(new EditTerms(selectedTerm));
        }

            // Deselect the item
            ((ListView)sender).SelectedItem = null;
    }
 
}