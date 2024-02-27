using CourseTracker.Maui.ViewModels;
using System;
using Android.Runtime;

namespace CourseTracker.Maui.Views;


[Preserve(AllMembers = true)]
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