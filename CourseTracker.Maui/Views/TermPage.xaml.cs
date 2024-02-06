using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CommunityToolkit.Maui.Converters;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Views;

public partial class TermPage : ContentPage
{
	TermVM viewModel;
    readonly Connection database = new Connection();
    readonly TermFactory _termFactory;
    public TermPage()
	{
		InitializeComponent();
		viewModel = new TermVM();
		BindingContext = viewModel;
	}

	public TermPage(Term termBeingEdited)
	{
        InitializeComponent();
        viewModel = new TermVM(termBeingEdited);
        BindingContext = viewModel;
	}

	private async void SubmitButton_Clicked(object sender, EventArgs e)
	{
        Debug.WriteLine(sender + " triggered this.");
		//TODO finish this
    }

	private async void CancelButton_Clicked(object sender, EventArgs e)
	{
        await Navigation.PopAsync();
    }
}