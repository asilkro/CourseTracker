using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class Homepage : ContentPage
{
    readonly HomepageVM viewModel;
    public Homepage()
    {
        InitializeComponent();
        BindingContext = viewModel = new HomepageVM();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        HomepageVM.OnAppearing();
    }

}