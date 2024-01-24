using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class EditTerms : ContentPage
{
	EditTermsVM viewModel;

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



    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine(sender + " triggered this.");
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine(sender + " triggered this.");
    }
}