using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.ViewModels
{
    public class ListInstructorsVM : ViewModelBase
    {
        public ObservableCollection<Instructor> Instructors { get; private set; } = new ObservableCollection<Instructor>();

        private Connection _database;
        public ListInstructorsVM()
        {
            LoadInstructors();
        }

        public async Task LoadInstructors()
        {
            try
            {
                _database = _database ?? new Connection();
                _database.GetAsyncConnection();
                var updatedInstructorsList = await _database.Table<Instructor>();
                Instructors.Clear();
                foreach (var instructor in updatedInstructorsList)
                {
                    Instructors.Add(instructor);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Issue loading instructors: " + ex.Message);
            }
        }

        private Instructor instructor = new();

        public Instructor Instructor
        {
            get { return instructor; }
            set
            {
                if (instructor != value)
                {
                    instructor = value;
                    OnPropertyChanged(nameof(Instructor));
                }
            }
        }
    }
}
