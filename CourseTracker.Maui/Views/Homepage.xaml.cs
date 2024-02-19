using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class Homepage : ContentPage
{
    HomepageVM viewModel;

    public Homepage()
    {
        InitializeComponent();
        BindingContext = viewModel = new HomepageVM();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }

}