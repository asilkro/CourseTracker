using System.Text.RegularExpressions;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using Plugin.LocalNotification;

namespace CourseTracker.Maui.Supplemental
{
    internal partial class Validation
    {
        static readonly DateTime _minDate = DateTime.Parse("01/01/2020");
        static readonly DateTime _maxDate = DateTime.Parse("12/31/4020");
        public TermsDB termsDB;
        public CourseDB courseDB;
        public AssessmentDB assessmentDB;

        public Validation()
        {
            termsDB = new TermsDB();
            courseDB = new CourseDB();
            assessmentDB = new AssessmentDB();
        }

        public static bool FirstOfTheMonth(DateTime date)
        {
            return date.Day == 1;
        }

        public static bool LastOfTheMonth(DateTime date)
        {
            return date.Day == DateTime.DaysInMonth(date.Year, date.Month);
        }

        public static bool TermsAreValid(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) return false;
            if (!DatesAreValid(startDate, endDate)) return false;

            return FirstOfTheMonth(startDate.Date) && LastOfTheMonth(endDate.Date);
        }

        public static bool DatesAreValid(DateTime start, DateTime end)
        {
            if (start < _minDate || end < _minDate) return false;
            if (start > _maxDate || end > _maxDate) return false;
            if (start >= end) return false;

            return true;
        }

        public static bool EmailIsValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool NotNull(string entry)
        {
            if (String.IsNullOrEmpty(entry) || String.IsNullOrWhiteSpace(entry))
            {
                return false;
            }

            return true;
        }

        public static bool ValidPhoneNumber(string phoneNumber)
        {
            Regex phoneFormat = MyRegex();
            return phoneFormat.IsMatch(phoneNumber);
        }

        public static bool ValidCourseAssessmentCount(int count)
        {
            if (2 >= count && count >= 0)
            {
                return true;
            }

            return false;
        }

        public static bool IdWasSet(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            return true;
        }

        public static bool CourseStatusIsValid(string status)
        {
            var result = status switch
            {
                "Planned" => true,
                "In Progress" => true,
                "Completed" => true,
                "Awaiting Evaluation" => true,
                "Dropped" => true,
                _ => false
            };
            return result;
        }


        public static bool AssessmentTypeIsValid(string assessmentType)
        {
            var result = assessmentType switch
            {
                "Objective" => true,
                "Performance" => true,
                _ => false
            };

            return result;
        }

        public static bool CourseCountIsValid(int courseCount)
        {
            if (courseCount < 0 || courseCount > 6)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> IsUniqueTermName(string termName, Connection _connection)
        {
            _connection ??= new Connection();
            var connection = _connection.GetAsyncConnection();
            var existingTerm = await connection.Table<Term>().Where(t => t.TermName == termName).FirstOrDefaultAsync();
            return existingTerm == null;
        }

        public static async Task<bool> IsUniqueCourseName(string courseName, Connection _connection)
        {
            _connection ??= new Connection();
            var connection = _connection.GetAsyncConnection();
            var existingCourse = await connection.Table<Course>().Where(c => c.CourseName == courseName).FirstOrDefaultAsync();
            return existingCourse == null;
        }

        public static async Task<bool> IsUniqueAssessmentName(string assessmentName, int courseId, Connection _connection)
        {
            _connection ??= new Connection();
            var connection = _connection.GetAsyncConnection();
            var existingAssessment = await connection.Table<Assessment>().Where(a => a.AssessmentName == assessmentName && a.RelatedCourseId == courseId).FirstOrDefaultAsync();
            return existingAssessment == null;
        }


        public async static Task<bool> IsValidNotification(NotificationRequest notification) //Non exhaustive checks
        {
            var result = true;
            if (notification == null)
            {
                result = false;
            }
            else if (!NotNull(notification.Title) || !NotNull(notification.Subtitle))
            {
                result = false;
            }
            else if (notification.Schedule == null)
            {
                result = false;
            }
            else if (notification.Schedule.NotifyTime <= DateTime.Now.Date)
            {
                result = false;
            }
            else if (notification.Schedule.NotifyAutoCancelTime < notification.Schedule.NotifyTime)
            {
                result = false;
            }

            return result;

        }

        public async Task<bool> DataExistsInTables()
        {
            List<Term> termCount = await termsDB.GetTermsAsync();
            List<Course> courseCount = await courseDB.GetCoursesAsync();
            List<Assessment> assessmentCount = await assessmentDB.GetAssessmentsAsync();

            var result = (termCount.Count > 0, courseCount.Count > 0, assessmentCount.Count > 0)
            switch
            {
                // If not all three are empty, return true to avoid conflicts
                (true, true, true) => true,
                (false, false, false) => false,
                _ => true,
            };
            return result;
        }

        [GeneratedRegex(@"^[\d-]+$")]
        private static partial Regex MyRegex();
    }

}
