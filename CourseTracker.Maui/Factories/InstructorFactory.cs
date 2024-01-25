using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;

namespace CourseTracker.Maui.Factories
{
    internal class InstructorFactory : FactoryBase<Instructor>
    {
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
            //TODO: Finish validation logic

            if (id <= 0)
            {
                errorMessage = "Instructor ID must be greater than 0.";
            }
            else if (Validation.IsNull(name))
            {
                errorMessage = "Instructor name cannot be empty.";
            }
            else if (!Validation.EmailIsValid(email))
            {
                errorMessage = "Instructor email is not valid.";
            }
            else if (Validation.IsNull(phone))
            {
                errorMessage = "Instructor phone cannot be empty.";
            }
            else if (!Validation.ValidPhoneNumber(phone))
            { 
                errorMessage = "Instructor phone is not valid."; 
            }

            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Instructor? CreateDefaultObject()
        {
            return new Instructor();
        }
    }
}
