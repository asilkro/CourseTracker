using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class EditTerms : ContentPage
{
	EditTermsVM viewModel;
    readonly Connection database = new Connection();

    public EditTerms()
	{
		InitializeComponent();
		viewModel = new EditTermsVM();
		BindingContext = viewModel;
	}

    public EditTerms(Term termBeingEdited)
    {
        InitializeComponent();
        viewModel = new EditTermsVM();
        BindingContext = viewModel;
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