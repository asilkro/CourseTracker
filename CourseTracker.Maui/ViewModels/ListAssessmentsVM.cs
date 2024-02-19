using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui.ViewModels
{
    public class ListAssessmentsVM : ViewModelBase
    {
        public ObservableCollection<Assessment> Assessments { get; private set; } = new ObservableCollection<Assessment>();

        public ListAssessmentsVM()
        {
        }

        public bool IsRefreshing { get; set; }
        public async Task LoadAssessments()
        {
            IsRefreshing = true;
            try
            {
                if (Assessments == null)
                {
                    Assessments = new ObservableCollection<Assessment>();
                }
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
                Debug.WriteLine("Issue loading assessments: " + ex.Message);
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
                    await assessmentDB.DeleteAssessmentAsync(assessment);
                    break;
                default:
                    break;
            }
        }

        private async void RemoveAssessmentAsync(Assessment assessment)
        {
            var result = await App.Current.MainPage.DisplayAlert("Delete Assessment", $"Are you sure you want to delete {assessment.AssessmentName}?", "Yes", "No");
            if (result)
            {
                int confirm = await assessmentDB.DeleteAssessmentAsync(assessment);
                //TODO: Update course assessment count
                if (confirm == 1)
                {
                    ShowToast("Assessment Deleted Successfully");
                }
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

        public void assessmentListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Assessment selectedAssessment)
            {
                ShowActionSheet(selectedAssessment);
            }
    ((ListView)sender).SelectedItem = null;
        }

        public async void OnAppearing()
        {
            await LoadAssessments();
        }
    }
}
