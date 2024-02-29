using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;

namespace CourseTracker.Maui.ViewModels
{
    public class HomepageVM : ViewModelBase
    {
        public Command OnLoadButtonClicked { get; set; }
        public Command OnResetDBButtonClicked { get; set; }
        public Command OnNotifyButtonClicked { get; set; }

        public ObservableCollection<Notification> Notifications { get; private set; } = [];
        private Connection _database;
        readonly NotifyDB notifyDB;

        public HomepageVM()
        {
            OnLoadButtonClicked = new Command(async () => await LoadButton_Clicked());
            OnResetDBButtonClicked = new Command(async () => await ResetDbButton_Clicked());
            OnNotifyButtonClicked = new Command(async () => await NotifyButton_Clicked());
            notifyDB = new NotifyDB();
        }
        private static async Task ResetDbButton_Clicked()
        {

            bool confirmed = await App.Current.MainPage.DisplayAlert("Reset Database", "This will delete ALL data in the database. Are you sure you want to continue?", "Yes", "No");
            if (confirmed)
            {
                await TrackerDb.ResetDatabaseFileAsync();
                ShowToast("Database has been reset, all data in tables has been removed.");
            }
        }
        private async Task LoadButton_Clicked()
        {
            await LoadSampleDataAsync();
        }

        private async Task NotifyButton_Clicked()
        {
            await LoadNotifications();
        }

        public async void OnAppearing()
        {
            notifyDB.NotificationRunner();
            Debug.WriteLine("Called Notification runner");
#if DEBUG
            await LoadNotifications();
            IsDebugEnabled = true;
#endif
        }

        private bool isDebugEnabled;
        public bool IsDebugEnabled
        {
            get => isDebugEnabled;
            set => SetProperty(ref isDebugEnabled, value);
        }

        private async Task LoadSampleDataAsync()
        {
            var validation = new Validation();
            var existing = await validation.DataExistsInTables();
            if (existing)
            {
                ShowToast("Please reset Database and restart app.");
                return;
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

                await App.Current.MainPage.DisplayAlert("Sample Data Loaded", "Sample data has been loaded successfully.", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sample Data Error", "$There was an error while loading sample data: " + ex.Message, "OK");
                return;
            }
        }

        #region Notifications

        private Notification notification;
        public Notification Notification
        {
            get => notification;
            set => SetProperty(ref notification, value);
        }

        private DateTime notificationDate;
        public DateTime NotificationDate
        {
            get => notificationDate;
            set => SetProperty(ref notificationDate, value);
        }

        private string notificationTitle;
        public string NotificationTitle
        {
            get => notificationTitle;
            set => SetProperty(ref notificationTitle, value);
        }

        private string notificationMessage;
        public string NotificationMessage
        {
            get => notificationMessage;
            set => SetProperty(ref notificationMessage, value);
        }

        public async Task LoadNotifications()
        {
            try
            {
                _database ??= new Connection();
                _database.GetAsyncConnection();
                var updatedNotificationList = await notifyDB.GetNotificationsAsync();
                Notifications.Clear();
                foreach (var notification in updatedNotificationList)
                {
                    Notifications.Add(notification);
                }
            }
            catch (Exception ex)
            {
                ShowToast("Issue loading notifications: " + ex.Message);
                return;
            }
        }

        #endregion

        #region Sample Data For Evaluation

        private async Task<Term> MakeDemoTerm()
        {
            Term demoTerm = new()
            {
                TermId = await termsDB.GetNextId(),
                TermName = "Evaluation Term",
                TermStart = new DateTime(2024, 03, 01),
                TermEnd = new DateTime(2024, 08, 31),
                CourseCount = 0
            };
            return demoTerm;
        }

        private async Task<Term> MakeDemoTerm2()
        {
            Term demoTerm2 = new()
            {
                TermId = await termsDB.GetNextId(),
                TermName = "Previous Term",
                TermStart = new DateTime(2023, 09, 01),
                TermEnd = new DateTime(2024, 02, 29),
                CourseCount = 0
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
                CourseStart = new DateTime(2024, 03, 01),
                CourseEnd = new DateTime(2024, 08, 31),
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
                CourseStart = new DateTime(2023, 09, 01),
                CourseEnd = new DateTime(2023, 11, 30),
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
                CourseStart = new DateTime(2023, 12, 01),
                CourseEnd = new DateTime(2024, 02, 29),
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
                AssessmentStartDate = new DateTime(2024, 03, 24),
                AssessmentEndDate = new DateTime(2024, 03, 27),
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
                AssessmentEndDate = new DateTime(2024, 03, 23),
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
                AssessmentStartDate = new DateTime(2023, 10, 01),
                AssessmentEndDate = new DateTime(2023, 10, 05),
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
                AssessmentStartDate = new DateTime(2024, 01, 04),
                AssessmentEndDate = new DateTime(2024, 01, 22),
                RelatedCourseId = 3,
                NotificationsEnabled = false
            };
            return demoPA2;
        }

        #endregion
    }
}