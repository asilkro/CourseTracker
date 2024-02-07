using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.ViewModels
{
    public class ListAssessmentsVM : ViewModelBase
    {
        private Connection _database;
        public ObservableCollection<Assessment> Assessments { get; private set; } = new ObservableCollection<Assessment>();

        public ListAssessmentsVM()
        {
            LoadAssessments();
        }

        public async Task LoadAssessments()
        {
            try
            {
                _database = _database ?? new Connection();
                _database.GetAsyncConnection();
                var updatedAssessmentsList = await _database.Table<Assessment>();
                Assessments.Clear();
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
                    OnPropertyChanged("Assessment");
                }
            }
        }
    }
}
