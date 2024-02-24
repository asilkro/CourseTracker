using System.Collections.ObjectModel;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui.ViewModels
{
    public class ListAssessmentsVM : ViewModelBase
    {
        public ObservableCollection<Assessment> Assessments { get; private set; } = [];
        public ObservableCollection<Course> Courses { get; private set; } = [];

        public ListAssessmentsVM()
        {
        }

        public bool IsRefreshing { get; set; }
        public async Task LoadAssessments()
        {
            IsRefreshing = true;
            try
            {
                Assessments ??= [];
                if (Assessments.Count > 0)
                {
                    Assessments.Clear();
                }
                var updatedAssessmentsList = await assessmentDB.GetAssessmentsAsync();
                foreach (var assessment in updatedAssessmentsList)
                {
                    Assessments.Add(assessment);
                }
            }
            catch (Exception ex)
            {
                ShowToast("Issue loading assessments: " + ex.Message);
                return;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private Assessment assessment = new();

        public Assessment Assessment
        {
            get { return assessment; }
            set
            {
                if (assessment != value)
                {
                    assessment = value;
                    OnPropertyChanged(nameof(Assessment));
                }
            }
        }

        private int assessmentId;
        public int AssessmentId
        {
            get { return assessmentId; }
            set
            {
                if (assessmentId != value)
                {
                    assessmentId = value;
                    OnPropertyChanged(nameof(AssessmentId));
                }
            }
        }

        private string assessmentName;
        public string AssessmentName
        {
            get { return assessmentName; }
            set
            {
                if (assessmentName != value)
                {
                    assessmentName = value;
                    OnPropertyChanged(nameof(AssessmentName));
                }
            }
        }

        private string assessmentType;
        public string AssessmentType
        {
            get { return assessmentType; }
            set
            {
                if (assessmentType != value)
                {
                    assessmentType = value;
                    OnPropertyChanged(nameof(AssessmentType));
                }
            }
        }

        private DateTime assessmentStartDate;
        public DateTime AssessmentStartDate
        {
            get { return assessmentStartDate; }
            set
            {
                if (assessmentStartDate != value)
                {
                    assessmentStartDate = value;
                    OnPropertyChanged(nameof(AssessmentStartDate));
                }
            }
        }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                SetProperty(ref _selectedCourse, value, onChanged: () =>
                {
                    if (_selectedCourse != null && value != null)
                    {
                        Assessment.RelatedCourseId = value.CourseId;
                    }
                });
            }
        }

        private DateTime assessmentEndDate;
        public DateTime AssessmentEndDate
        {
            get { return assessmentEndDate; }
            set
            {
                if (assessmentEndDate != value)
                {
                    assessmentEndDate = value;
                    OnPropertyChanged(nameof(AssessmentEndDate));
                }
            }
        }

        private int relatedCourseId;
        public int RelatedCourseId
        {
            get { return relatedCourseId; }
            set
            {
                if (relatedCourseId != value)
                {
                    relatedCourseId = value;
                    OnPropertyChanged(nameof(RelatedCourseId));
                }
            }
        }

        private bool notificationsEnabled;
        public bool NotificationsEnabled
        {
            get { return notificationsEnabled; }
            set
            {
                if (notificationsEnabled != value)
                {
                    notificationsEnabled = value;
                    OnPropertyChanged(nameof(NotificationsEnabled));
                }
            }
        }

        public async void ShowActionSheet(Assessment assessment)
        {
            string action = await App.Current.MainPage.DisplayActionSheet("Assessment Actions", "Cancel", null, "Edit Assessment", "Delete Assessment");
            switch (action)
            {
                case "Edit Assessment":
                    await Shell.Current.GoToAsync($"{nameof(AssessmentPage)}?{nameof(AssessmentVM.EditAssessmentId)}={assessment.AssessmentId}");
                    break;
                case "Delete Assessment":
                    bool answer = await Application.Current.MainPage.DisplayAlert("Delete Assessment", "Are you sure you want to delete this assessment?", "Yes", "No");
                    if (answer)
                    {
                        var course = await courseDB.GetCourseByIdAsync(assessment.RelatedCourseId);
                        await assessmentDB.DeleteAssessmentAsync(assessment);
                        await sharedDB.UpdateCourseAssessmentCount(course);
                        await LoadAssessments();
                        await Shell.Current.GoToAsync("..");
                    }
                    break;
                default:
                    break;
            }
        }

        public async Task<string> LoadCoursesAsync()
        {
            try
            {
                var courses = await courseDB.GetCoursesAsync();
                Assessment assessment = await assessmentDB.GetAssessmentByIdAsync(AssessmentId);
                assessment ??= new Assessment();
                Courses.Clear();
                Course course1 = new();
                foreach (var course in courses)
                {
                    if (course.CourseId == assessment.RelatedCourseId)
                    {
                        course1 = course;
                    }
                    Courses.Add(course);
                }
                SelectedCourse = course1;
                return SelectedCourse.CourseName;
            }
            catch (Exception ex)
            {
                return "Issue loading courses: " + ex.Message;
            }
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Assessment selectedAssessment)
            {
                ShowActionSheet(selectedAssessment);
            }
        ((ListView)sender).SelectedItem = null;
        }

        public void AssessmentListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Assessment selectedAssessment)
            {
                ShowActionSheet(selectedAssessment);
            }
    ((ListView)sender).SelectedItem = null;
        }

        public async Task OnAppearing()
        {
            await LoadAssessments();
        }
    }
}
