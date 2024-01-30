using CourseTracker.Maui.Models;
using CourseTracker.Maui.Placeholder_Stuff;
using CourseTracker.Maui.Supplemental;
using System.Diagnostics;
using CourseTracker.Maui.Services;
using System.Diagnostics.Metrics;

namespace CourseTracker.Maui.Factories
{
    public class InstructorFactory : FactoryBase<Instructor>
    {
        private readonly DummyData _dummyData;
        public InstructorFactory(IAsyncSqLite database, DummyData dummyData) : base(database)
        {
            _dummyData = dummyData;
        }

        public Instructor? CreateInstructor(int id, string name, string email, string phone, out string errorMessage)
        {
            if (!IsValidInstructor(id, name, email, phone, out errorMessage))
            {
                return null;
            }
            var Instructor = CreateObject();
            Instructor.InstructorId = id;
            Instructor.InstructorName = name;
            Instructor.InstructorEmail = email;
            Instructor.InstructorPhone = phone;
            return Instructor;
        }

        public bool IsValidInstructor(int id, string name, string email,
            string phone, out string errorMessage)
        {
            errorMessage = "";

            if (!Validation.IdWasSet(id))
            {
                errorMessage = "Instructor ID must be greater than 0.";
            }
            else if (!Validation.NotNull(name))
            {
                errorMessage = "Instructor name cannot be empty.";
            }
            else if (!Validation.EmailIsValid(email))
            {
                errorMessage = "Instructor email is not valid.";
            }
            else if (!Validation.NotNull(phone))
            {
                errorMessage = "Instructor phone cannot be empty.";
            }
            else if (!Validation.ValidPhoneNumber(phone))
            { 
                errorMessage = "Instructor phone is not valid."; 
            }

            Debug.WriteLine(errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Instructor? CreateDefaultObject()
        {
            return new Instructor();
        }

        public async Task<List<Instructor>> GenerateSampleInstructors(int numberOfInstructors)
        {
            var instructors = new List<Instructor>();
            // Loop to create the specified number of sample instructors
            for (int i = 0; i < numberOfInstructors; i++)
            {
                // Generate sample instructors
                var instructor = new Instructor
                {
                    InstructorId = i + 1,
                    InstructorName = _dummyData.InstructorNames[i % _dummyData.InstructorNames.Count],
                    InstructorEmail = _dummyData.InstructorEmails[i % _dummyData.InstructorEmails.Count],
                    InstructorPhone = _dummyData.InstructorPhones[i % _dummyData.InstructorPhones.Count]
                };

                if (!IsValidInstructor(instructor.InstructorId, instructor.InstructorName,
                                       instructor.InstructorEmail, instructor.InstructorPhone, out string errorMessage))
                { 
                    return instructors;
                }

                // Add the sample instructors to the list
                instructors.Add(instructor);

                // Add the sample instructors to the database
                await AddObject(instructor);
            }

            return instructors;
        }
    }
}
