namespace CourseTracker.Maui.Views;

public partial class TermPage : ContentPage
{
	public TermPage()
	{
		InitializeComponent();
	}

	private async void SubmitButton_Clicked(object sender, EventArgs e)
	{
        //Something
    }

	private async void CancelButton_Clicked(object sender, EventArgs e)
	{
        await Navigation.PopAsync();
    }
}