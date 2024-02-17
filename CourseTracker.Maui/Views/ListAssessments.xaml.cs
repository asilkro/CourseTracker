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

}