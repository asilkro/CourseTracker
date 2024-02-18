using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class TermPage : ContentPage
{
    readonly TermVM viewModel;

    public TermPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new TermVM();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}