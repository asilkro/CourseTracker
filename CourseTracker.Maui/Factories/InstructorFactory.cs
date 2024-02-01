using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;
using System.Diagnostics;
using CourseTracker.Maui.Services;
using System.Diagnostics.Metrics;

namespace CourseTracker.Maui.Factories
{
    public class InstructorFactory : FactoryBase<Instructor>
    {
        public InstructorFactory(IAsyncSqLite database) : base(database)
        {
            
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
                errorMessage = "Instructor ID must be greater than 0.";

            else if (!Validation.NotNull(name))
                errorMessage = "Instructor name cannot be empty.";

            else if (!Validation.EmailIsValid(email))
                errorMessage = "Instructor email is not valid.";

            else if (!Validation.NotNull(phone))
                errorMessage = "Instructor phone cannot be empty.";

            else if (!Validation.ValidPhoneNumber(phone))
                errorMessage = "Instructor phone is not valid.";

            Debug.WriteLine(errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Instructor? CreateDefaultObject()
        {
            return new Instructor();
        }

    }
}
