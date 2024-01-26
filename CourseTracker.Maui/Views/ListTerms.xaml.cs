using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class ListTerms : ContentPage
{
	ListTermsVM viewModel;

	public ListTerms()
    {
        InitializeComponent();
		viewModel = new ListTermsVM();
		BindingContext = viewModel;
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