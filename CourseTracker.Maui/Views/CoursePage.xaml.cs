using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.Factories;

namespace CourseTracker.Maui.Views;

public partial class CoursePage : ContentPage
{
    CourseVM viewModel;
    readonly Connection database = new();
    readonly CourseFactory _courseFactory;
    int nextCourseId = TrackerDb.GetNextAutoIncrementID("Course");
    public CoursePage()
    {
        InitializeComponent();
        viewModel = new CourseVM();
        this.BindingContext = viewModel;
        courseIdEntry.Text = nextCourseId.ToString();
        courseIdEntry.IsReadOnly = true;
    }

    public CoursePage(Course course)
    {
        InitializeComponent();
        viewModel = new CourseVM(course);
        BindingContext = viewModel;
        courseIdEntry.Text = course.CourseId.ToString();
        courseIdEntry.IsReadOnly = true;
    }

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        var courseResult = await _courseFactory.CreateCourseAsync(viewModel);

        if (courseResult?.Course == null) 
        {
            Debug.WriteLine("Error creating course: " + courseResult?.ErrorMessage);
            return;
        }

        var course = courseResult.Course;
        var searchId = course.CourseId;

        try
        {
            var exists = database.FindAsync<Course>(searchId);
            if (exists == null) 
            {
                await _courseFactory.InsertCourseAndUpdateTermCount(course);
                if (course.NotificationsEnabled)
                {
                    await _courseFactory.ScheduleCourseNotifications(course);
                }
            }
            else
            {
                await _courseFactory.UpdateCourseAndUpdateTermCount(course);
                if (course.NotificationsEnabled)
                {
                    await _courseFactory.ScheduleCourseNotifications(course);
                }
            }
            bool anotherCourseWanted = await DisplayAlert("Course Added", "Would you like to add another course?", "Yes", "No");
            if (anotherCourseWanted)
            {
                await Shell.Current.GoToAsync("//coursepage");
            }
            else
            {
                await Shell.Current.GoToAsync("//homepage");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error submitting course: " + ex.Message);
        }
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homepage");
    }

    private void courseNoteShareButton_Clicked(object sender, EventArgs e)
    {
        var note = viewModel.Course.CourseNotes;
        var entry = courseNoteEditor.Text;
        var course = viewModel.Course.CourseName;

        Debug.WriteLine("Course.CourseNotes = " + note);
        Debug.WriteLine("courseNoteEditor.Text = " + entry);

        if (!Validation.NotNull(note))
        {
            DisplayAlert("Note Validation Error", "No course notes found to share.", "OK");
        }
        else
        {
            ShareText(note, course);
            //ShareText(entry, course);
        }
    }

    public async Task ShareText(string notes, string source) //
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = notes + Environment.NewLine + "Shared on " + DateTime.Now.ToShortDateString(),
            Title = "Course Notes for " + source,
        });
    }
}