using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Placeholder_Stuff;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Views;

public partial class Homepage : ContentPage
{
	TrackerDb _trackerDb = new TrackerDb();
	public Homepage()
	{
		InitializeComponent();
		StartDB();
	}

	private async Task StartDB()
	{
        await TrackerDb.Initialize();
    }

	private async void LoadDummyData(string type, int howMany)
	{
        Debug.WriteLine("Attempting to load " + howMany + " sample item(s) of type " + type);
        switch (type)
		{
			case "Assessment":
				var assessmentFactory = new AssessmentFactory(new Connection(), new DummyData());
				await assessmentFactory.GenerateSampleTerms(howMany);
				break;
			case "Course":
				var courseFactory = new CourseFactory(new Connection(), new DummyData());
				await courseFactory.GenerateSampleCourses(howMany);
				break;
			case "Instructor":
                var instructorFactory = new InstructorFactory(new Connection(), new DummyData());
                await instructorFactory.GenerateSampleInstructors(howMany);
                break;
			case "Term":
				var termFactory = new TermFactory(new Connection(), new DummyData());
                await termFactory.GenerateSampleTerms(howMany);
                break;
		}
		Debug.WriteLine("Insert successful(?) of " + howMany + 
			" sample item(s) of type " + type);
	}

    private void loadButton_Clicked(object sender, EventArgs e)
    {
		var number = dummyButtonSlider.Value;
		if (number < 1) { return; }
		if (sender == loadAssessmentButton)
		{
			LoadDummyData("Assessment", Convert.ToInt32(number));
		}
		else if (sender == loadCourseButton)
		{
			LoadDummyData("Course", Convert.ToInt32(number));
		}
		else if (sender == loadInstructorButton)
		{
			LoadDummyData("Instructor", Convert.ToInt32(number));
		}
		else if (sender == loadTermButton)
		{
			LoadDummyData("Term", Convert.ToInt32(number));
		}

    }
}