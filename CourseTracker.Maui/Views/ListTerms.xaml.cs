using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace CourseTracker.Maui.Views;

public partial class ListTerms : ContentPage
{
    private readonly ListTermsVM _listTermsVM;
    public ListTerms()
    {
        InitializeComponent();
        BindingContext = _listTermsVM = new ListTermsVM();
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _listTermsVM.OnAppearing();
    }
}