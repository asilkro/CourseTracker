using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CommunityToolkit.Maui.Converters;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Data;

namespace CourseTracker.Maui.Views;

public partial class TermPage : ContentPage
{
	TermVM viewModel;
	
    public TermPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new TermVM();

        termIdEntry.IsReadOnly = true; // keep this from being edited
        termCourseCountEntry.IsReadOnly = true; // Function updates this
    }

}