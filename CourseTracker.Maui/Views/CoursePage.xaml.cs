using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;

namespace CourseTracker.Maui.Views;

public partial class CoursePage : ContentPage
{
    CourseVM viewModel;
    readonly Connection database = new();
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

    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine(sender + " triggered this.");
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
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