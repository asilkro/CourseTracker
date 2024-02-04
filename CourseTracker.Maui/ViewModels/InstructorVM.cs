using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;
using System.Windows.Input;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Supplemental;

namespace CourseTracker.Maui.ViewModels
{
    public class InstructorVM : ViewModelBase
    {
        private int _instructorId = -1;
        private string _instructorName = string.Empty;
        private string _instructorEmail = string.Empty;
        private string _instructorPhone = string.Empty;
        private Connection connection = new Connection();

        private Instructor instructor;
        private Instructor _instructor = new Instructor();

        public Instructor Instructor
        {
            get => _instructor;
            set
            {
                if (_instructor != value)
                {
                    _instructor = value;
                    OnPropertyChanged(nameof(Instructor));
                }
            }
        }

        public InstructorVM() 
        {
            // Blank constructor
        }

        public InstructorVM(int instructorId)
        {
            LoadInstructorDetails(instructorId);
        }

        private async void LoadInstructorDetails(int instructorId)
        {
            var _database = new Connection();
            Instructor = await _database.FindAsync<Instructor>(instructorId);
        }

        public ICommand SaveInstructorCommand => new Command(async () => await SaveInstructorAsync());

        private async Task SaveInstructorAsync()
        {
            InstructorFactory InstructorFactory = new(connection);

            if (InstructorFactory.IsValidInstructor(_instructorId, _instructorName, _instructorEmail, _instructorPhone, out _)) // Valid value
            {
                connection.GetAsyncConnection();
                await connection.InsertAndGetIdAsync<Instructor>(_instructor);
             
            }
            else
            {
                // Show validation error message
            }
        }


        public int InstructorId
        {
            get { return _instructorId; }
            set
            {
                if (_instructorId != value)
                {
                    _instructorId = value;
                    OnPropertyChanged("InstructorId");
                }
            }
        }

        public string InstructorName
        {
            get { return _instructorName; }
            set
            {
                if (_instructorName != value)
                {
                    _instructorName = value;
                    OnPropertyChanged("InstructorName");
                }
            }
        }

        public string InstructorEmail
        {
            get { return _instructorEmail; }
            set
            {
                if (_instructorEmail != value)
                {
                    _instructorEmail = value;
                    OnPropertyChanged("InstructorEmail");
                }
            }
        }

        public string InstructorPhone
        {
            get { return _instructorPhone; }
            set
            {
                if (_instructorPhone != value)
                {
                    _instructorPhone = value;
                    OnPropertyChanged("InstructorPhone");
                }
            }
        }

    }
}
