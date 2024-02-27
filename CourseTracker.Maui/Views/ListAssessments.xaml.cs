using Android.Runtime;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

[Preserve(AllMembers = true)]
public partial class ListAssessments : ContentPage
{
    private readonly ListAssessmentsVM viewModel;

    public ListAssessments()
    {
        InitializeComponent();
        BindingContext = viewModel = new ListAssessmentsVM();
    }

    public void AssessmentListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Assessment selectedAssessment)
        {
            viewModel.ShowActionSheet(selectedAssessment);
        }
        ((ListView)sender).SelectedItem = null;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}