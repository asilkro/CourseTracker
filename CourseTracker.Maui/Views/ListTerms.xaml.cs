using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Factories;

namespace CourseTracker.Maui.Views;

public partial class ListTerms : ContentPage
{
	private ListTermsVM viewModel;
    private Connection _database;
    readonly TermFactory _termFactory;

    public ListTerms()
    {
        InitializeComponent();
        viewModel = new ListTermsVM();
        BindingContext = viewModel;
        InitializeDataAsync();
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