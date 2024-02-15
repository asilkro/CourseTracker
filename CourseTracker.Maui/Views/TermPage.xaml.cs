using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CommunityToolkit.Maui.Converters;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Data;

namespace CourseTracker.Maui.Views;

public partial class TermPage : ContentPage
{
	TermVM viewModel;
	
    public TermPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new TermVM();
        //termIdEntry.Text = nextTermId.ToString();

        termIdEntry.IsReadOnly = true; // keep this from being edited
        termCourseCountEntry.IsReadOnly = true; // Function updates this
    }

    private async void SubmitButton_Clicked(object sender, EventArgs e)
	{
		try
		{
            /* if (_termFactory == null)
             {
                 _termFactory = new TermFactory(database);
             }
             var term = _termFactory.CreateTerm(viewModel, out string errorMessage);
             if (term == null)
             {
                 Debug.WriteLine(errorMessage);
                 return;
             }
             var exists = await database.FindAsync<Term>(term.TermId);

             switch (exists)
             {
                 case null:
                     await database.InsertAsync<Term>(term);
                     break;
                 default:
                     await database.UpdateAsync<Term>(term);
                     break;
             }*/
        }
        catch (Exception ex)
		{
            Debug.WriteLine(ex.Message);
        }
/*		
		if (viewModel.TermId <= 0)
		{
			if(_termFactory.IsValidTerm(viewModel.TermId, viewModel.TermName, viewModel.TermStart, viewModel.TermEnd, viewModel.CourseCount, out string errorMessage))
			{
                await database.InsertAndGetIdAsync(viewModel.Term);
            }
            else
			{
                Debug.WriteLine(errorMessage);
            }
		}
		else
		{
			await database.UpdateAsync(viewModel.Term);
		}*/
        bool anotherTermWanted = await DisplayAlert("Term Saved", "Would you like to add another term?", "Yes", "No");
        if (anotherTermWanted)
        {
            await Shell.Current.GoToAsync(nameof(TermPage));
        }
        await Shell.Current.GoToAsync("//homepage");
    }

	private async void CancelButton_Clicked(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync("//homepage");
    }
}