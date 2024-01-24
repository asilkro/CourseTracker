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
        Console.WriteLine(sender + " triggered this.");
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine(sender + " triggered this.");
    }
}