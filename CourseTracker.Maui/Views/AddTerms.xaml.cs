using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CommunityToolkit.Maui.Converters;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Views;

public partial class AddTerms : ContentPage
{
    AddTermsVM viewModel;
    readonly Connection database = new Connection();
    readonly TermFactory _termFactory;
    public AddTerms(TermFactory termFactory)
	{
		InitializeComponent();
        viewModel = new AddTermsVM();
        BindingContext = viewModel;
        _termFactory = termFactory;
	}

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine(sender + " triggered this.");
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}