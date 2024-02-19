using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using Newtonsoft.Json;

namespace CourseTracker.Maui.ViewModels
{
    public class HomepageVM : ViewModelBase
    {
        public Command OnLoadButtonClicked { get; set; }
        public Command OnResetDBButtonClicked { get; set; }
        public HomepageVM()
        {
            OnLoadButtonClicked = new Command(async () => await loadButton_Clicked());
            OnResetDBButtonClicked = new Command(async () => await resetDbButton_Clicked());
        }
        private async Task StartDB()
        {
            ShowToast("Initializing application");
        }
        private async Task resetDbButton_Clicked()
        {
            bool confirmed = await App.Current.MainPage.DisplayAlert("Reset Database", "This will delete all data in the database. Are you sure you want to continue?", "Yes", "No");
            if (confirmed)
            {
                await TrackerDb.ResetDatabaseFileAsync();
            }
        }
        private async Task loadButton_Clicked()
        {
            Debug.WriteLine("Starting to load Data");
            await LoadSampleDataAsync();
        }
        public async void OnAppearing()
        {
            await StartDB();
        }

        private async Task LoadSampleDataAsync()
        {
            var validation = new Validation();
            var existing = await validation.DataExistsInTables();
            Debug.WriteLine("Existing check: " + existing);
            if (existing)
            {
                //await DisplayAlert("Table Already Has Data", "Table data has already been loaded. " +
                //    "You should reset the database to avoid errors with sample data creation.", "OK");
                //return;
            }

            try
            {
                //Term, Course and Assessments for C6 and C3 requirements

                var DemoTerm = await MakeDemoTerm();

                await sharedDB.SaveTerm(DemoTerm);
                Debug.WriteLine("Inserted term: " + DemoTerm.TermName);

                var demoCourse = await MakeDemoCourse1();
                demoCourse.TermId = DemoTerm.TermId;
                                
                await sharedDB.InsertCourseAndUpdateTerm(demoCourse);
                Debug.WriteLine("Inserted course: " + demoCourse.CourseName);

                var demoOA = await MakeDemoOA(); //Assessment for course 1
                demoOA.RelatedCourseId = demoCourse.CourseId;
                await sharedDB.SaveAssessmentAndUpdateCourse(demoOA); //1
                Debug.WriteLine("Inserted assessment: " + demoOA.AssessmentName); //C6 OA
                                
                var demoPA = await MakeDemoPA(); //Assessment for course 1
                demoPA.RelatedCourseId = demoCourse.CourseId;
                await sharedDB.SaveAssessmentAndUpdateCourse(demoPA); //2
                Debug.WriteLine("Inserted assessment: " + demoPA.AssessmentName); //C6 PA

                //Second term and courses to provide a more robust demo set of data for evaluator
                var demoTerm2 = await MakeDemoTerm2();
                await sharedDB.SaveTerm(demoTerm2);
                Debug.WriteLine("Inserted term: " + demoTerm2.TermName);

                var demoCourse2 = await MakeDemoCourse2(); //Course created
                demoCourse2.TermId = demoTerm2.TermId;

                await sharedDB.InsertCourseAndUpdateTerm(demoCourse2);
                Debug.WriteLine("Inserted course: " + demoCourse2.CourseName);  

                var demoOA2 = await MakeDemoOA2();
                demoOA2.RelatedCourseId = demoCourse2.CourseId;
                await sharedDB.SaveAssessmentAndUpdateCourse(demoOA2); //3
                Debug.WriteLine("Inserted assessment:" + demoOA2.AssessmentName);

                var demoCourse3 = await MakeDemoCourse3(); //Course created
                demoCourse3.TermId = demoTerm2.TermId;
                await sharedDB.InsertCourseAndUpdateTerm(demoCourse3);
                Debug.WriteLine("Inserted course: " + demoCourse3.CourseName);

                var demoPA2 = await MakeDemoPA2();
                demoPA2.RelatedCourseId = demoCourse3.CourseId;  
                await sharedDB.SaveAssessmentAndUpdateCourse(demoPA2); //4
                Debug.WriteLine("Inserted assessment: " + demoPA2.AssessmentName);
                
                await App.Current.MainPage.DisplayAlert("Sample Data Loaded", "Sample data has been loaded successfully. Please relaunch the application to complete setup.", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sample Data Error", "$There was an error while loading sample data: " + ex.Message, "OK");
                return;
            }
        }
        #region Sample Data For Evaluation

        private async Task<Term> MakeDemoTerm()
        {
            Term demoTerm = new()
            {
                TermId = 1,//await termsDB.GetNextId(),
                TermName = "Demo Term",
                TermStart = new DateTime(2024, 01, 01),
                TermEnd = new DateTime(2024, 06, 30),
                CourseCount = 1
            };
            return demoTerm;
        }

        private async Task<Term> MakeDemoTerm2()
        {
            Term demoTerm2 = new()
            {
                TermId = await termsDB.GetNextId(),
                TermName = "Term One 2023",
                TermStart = new DateTime(2023, 01, 01),
                TermEnd = new DateTime(2023, 06, 30),
                CourseCount = 2
            };
            return demoTerm2;
        }


        private async Task<Course> MakeDemoCourse1()
        {
            Course course = new()
            {
                CourseId = await courseDB.GetNextId(),
                CourseName = "Example Course for Evaluation",
                CourseStatus = "In Progress",
                CourseStart = new DateTime(2024, 01, 01),
                CourseEnd = new DateTime(2024, 06, 30),
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                InstructorPhone = "555-123-4567",
                InstructorName = "Anika Patel",
                CourseNotes = "This addresses assessment requirement C6 re: C3",
                NotificationsEnabled = true,
                TermId = 1,
                CourseAssessmentCount = 0
            };

            return course;
        }

        private async Task<Course> MakeDemoCourse2()
        {
            Course course = new()
            {
                CourseId = await courseDB.GetNextId(),
                CourseName = "Relationships in a Digital Era",
                CourseStatus = "Completed",
                CourseStart = new DateTime(2023, 01, 01),
                CourseEnd = new DateTime(2023, 02, 28),
                InstructorEmail = "zenobia@subbrat.edu",
                InstructorPhone = "554-207-6943",
                InstructorName = "Marielle CC Zenobia",
                CourseNotes = "This course covered the change in relationships amongst Gen Z and millenials in the era of dating apps.",
                NotificationsEnabled = false,
                TermId = 2,
                CourseAssessmentCount = 0
            };
            return course;
        }

        private async Task<Course> MakeDemoCourse3()
        {
            Course course = new()
            {
                CourseId = await courseDB.GetNextId(),
                CourseName = "Screen Writing for Community",
                CourseStatus = "Dropped",
                CourseStart = new DateTime(2023, 03, 01),
                CourseEnd = new DateTime(2023, 04, 30),
                InstructorEmail = "dharmon@greendale.edu",
                InstructorPhone = "555-424-1565",
                InstructorName = "Dan Harmon",
                CourseNotes = "This course was dropped due to a scheduling conflict with Dean Pelton.",
                NotificationsEnabled = false,
                TermId = 2,
                CourseAssessmentCount = 0
            };
            return course;
        }

        private async Task<Assessment> MakeDemoOA()
        {
            Assessment demoOA = new()
            {
                AssessmentId = await assessmentDB.GetNextId(),
                AssessmentName = "C6 OA",
                AssessmentType = "Objective",
                AssessmentStartDate = new DateTime(2024, 01, 01),
                AssessmentEndDate = new DateTime(2024, 02, 29),
                RelatedCourseId = 1,
                NotificationsEnabled = true
            };
            return demoOA;
        }

        private async Task<Assessment> MakeDemoPA()
        {
            Assessment demoPA = new()
            {
                AssessmentId = await assessmentDB.GetNextId(),
                AssessmentName = "C6 PA",
                AssessmentType = "Performance",
                AssessmentStartDate = new DateTime(2024, 03, 01),
                AssessmentEndDate = new DateTime(2024, 04, 30),
                RelatedCourseId = 1,
                NotificationsEnabled = true
            };
            return demoPA;
        }

        private async Task<Assessment> MakeDemoOA2()
        {
            Assessment demoOA2 = new()
            {
                AssessmentId = await assessmentDB.GetNextId(),
                AssessmentName = "Digital Relationships OA",
                AssessmentType = "Objective",
                AssessmentStartDate = new DateTime(2023, 01, 01),
                AssessmentEndDate = new DateTime(2023, 02, 28),
                RelatedCourseId = 2,
                NotificationsEnabled = false
            };
            return demoOA2;
        }

        private async Task<Assessment> MakeDemoPA2()
        {
            Assessment demoPA2 = new()
            {
                AssessmentId = await assessmentDB.GetNextId(),
                AssessmentName = "Community Script PA",
                AssessmentType = "Performance",
                AssessmentStartDate = new DateTime(2023, 03, 01),
                AssessmentEndDate = new DateTime(2024, 02, 29),
                RelatedCourseId = 3,
                NotificationsEnabled = false
            };
            return demoPA2;
        }


        #endregion
    }

}