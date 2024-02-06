using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.ViewModels
{
    public class CourseVM : ViewModelBase
    {
        private int courseId;
        private Course course;

        public CourseVM(Course course)
        {
            courseId = course.CourseId;
            LoadCourseDetails();
        }

        public CourseVM()
        {
            LoadTerms();
        }

        public async Task InitializeAsync()
        {
            if (courseId > 0)
            {
                await LoadCourseDetails();
            }
            else
            {
                await LoadTerms();
            }
        }

        public Course Course
        {
            get => course;
            set
            {
                if (course != value)
                {
                    course = value;
                    OnPropertyChanged(nameof(Course));
                }
            }
        }


        private async Task LoadCourseDetails()
        {

            Connection DatabaseService = new();
            DatabaseService.GetAsyncConnection();

            if (courseId > 0)
            {
                Course = await DatabaseService.FindAsync<Course>(courseId);
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

        private DateTime minimumDate = DateTime.Parse("01/01/2020");
        public DateTime MinimumDate
        {
            get { return minimumDate; }
            set
            {
                if (minimumDate != value)
                {
                    minimumDate = value;
                    OnPropertyChanged("MinimumDate");
                }
            }
        }

        private DateTime maximumDate = DateTime.Parse("12/31/4020");
        public DateTime MaximumDate
        {
            get { return maximumDate; }
            set
            {
                if (maximumDate != value)
                {
                    maximumDate = value;
                    OnPropertyChanged("MaximumDate");
                }
            }
        }

        private DateTime date = DateTime.Now.Date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private Term term = new();
        public Term Term
        {
            get { return term; }
            set
            {
                if (term != value)
                {
                    term = value;
                    OnPropertyChanged("Term");
                }
            }
        }

        public ObservableCollection<Term> Terms { get; } = new ObservableCollection<Term>();
        Connection _connection;


        private async Task LoadTerms()
        {
            try
            {
                _connection = _connection ?? new Connection();
                _connection.GetAsyncConnection();
                var terms = await _connection.Table<Term>();
                Terms.Clear();
                foreach (var term in terms)
                {
                    Terms.Add(term);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Issue loading terms: " + ex.Message);
            }
        }



        private Term _selectedTerm;
        public Term SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                if (_selectedTerm != value)
                {
                    _selectedTerm = value;
                    Course.TermId = value.TermId;
                    OnPropertyChanged(nameof(SelectedTerm));
                }
            }
        }
    }
}