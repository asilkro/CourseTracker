using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Factories;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTracker.Maui.Views;

public partial class AssessmentPage : ContentPage
{
	AssessmentVM viewModel;

	public AssessmentPage()
	{
		InitializeComponent();
        BindingContext = viewModel = new AssessmentVM();
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        viewModel.OnAppearing();
    }

}