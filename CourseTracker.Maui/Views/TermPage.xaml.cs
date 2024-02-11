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
    readonly Connection database = new();
    readonly TermFactory _termFactory;
	int nextTermId = TrackerDb.GetNextAutoIncrementID("Term");
    public TermPage()
	{
		InitializeComponent();
		database.GetAsyncConnection();
		viewModel = new TermVM();
		BindingContext = viewModel;
		termIdEntry.Text = nextTermId.ToString();
        termIdEntry.IsReadOnly = true; // keep this from being edited
    }

	public TermPage(Term termBeingEdited)
	{
        InitializeComponent();
        database.GetAsyncConnection();
        viewModel = new TermVM(termBeingEdited);
        BindingContext = viewModel;
		termIdEntry.Text = termBeingEdited.TermId.ToString();
        termIdEntry.IsReadOnly = true; // keep this from being edited
    }

	private async void SubmitButton_Clicked(object sender, EventArgs e)
	{
		try
		{
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
            }
        }
        catch (Exception ex)
		{
            Debug.WriteLine(ex.Message);
        }
		
		if (viewModel.TermId <= 0)
		{
			if(_termFactory.IsValidTerm(viewModel.TermId, viewModel.TermName, viewModel.TermStart, viewModel.TermEnd, viewModel.NotificationsEnabled, viewModel.CourseCount, out string errorMessage))
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
		}
        await Navigation.PopToRootAsync();
    }

	private async void CancelButton_Clicked(object sender, EventArgs e)
	{
        await Navigation.PopToRootAsync();
    }
}