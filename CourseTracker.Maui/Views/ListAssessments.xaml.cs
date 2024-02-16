using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using System.Collections.ObjectModel;
using CourseTracker.Maui.Factories;
using System.Diagnostics;

namespace CourseTracker.Maui.Views;

public partial class ListAssessments : ContentPage
{
    private ListAssessmentsVM viewModel;
    private Connection _database;
    private ObservableCollection<Assessment> _assessments = new();

    public ListAssessments()
    {
        InitializeComponent();
        BindingContext = viewModel = new ListAssessmentsVM();
    }

}