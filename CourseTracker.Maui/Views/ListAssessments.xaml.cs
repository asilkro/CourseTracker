using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class ListAssessments : ContentPage
{
    private readonly ListAssessmentsVM viewModel;

    public ListAssessments()
    {
        InitializeComponent();
        BindingContext = viewModel = new ListAssessmentsVM();
    }

    public void assessmentListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Assessment selectedAssessment)
        {
            viewModel.ShowActionSheet(selectedAssessment);
        }
        ((ListView)sender).SelectedItem = null;
    }
}