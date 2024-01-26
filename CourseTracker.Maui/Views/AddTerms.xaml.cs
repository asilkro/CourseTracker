using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class AddTerms : ContentPage
{
    AddTermsVM viewModel;

	public AddTerms()
	{
		InitializeComponent();
        viewModel = new AddTermsVM();
        BindingContext = viewModel;
	}

    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        var factory = new TermFactory();
        factory.CreateTerm(viewModel, out var errorMessage);

        Debug.WriteLine(sender + " triggered this.");
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}