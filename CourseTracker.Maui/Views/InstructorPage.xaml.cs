using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Views;

public partial class InstructorPage : ContentPage
{
	InstructorVM viewModel;
    readonly Connection database = new();
    public InstructorPage()
	{
        InitializeComponent();
        viewModel = new InstructorVM();
        this.BindingContext = viewModel;
    }

    public InstructorPage(Instructor? instructor)
    {
        InitializeComponent();
        viewModel = new InstructorVM();
        this.BindingContext = viewModel;
        if (instructor != null)
        {
            setInstructor(instructor);
        }
    }

    private void setInstructor(Instructor instructor)
    {
        viewModel.Instructor = instructor;
    }
}