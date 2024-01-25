using System.Text.RegularExpressions;

namespace CourseTracker.Maui.Supplemental
{
    internal class Validation
    {
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
            
            return FirstOfTheMonth(startDate.Date) && LastOfTheMonth(endDate.Date);
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

        public static bool IsNull(string entry)
        {
            if (String.IsNullOrEmpty(entry) || String.IsNullOrWhiteSpace(entry))
            {
                return true;
            }

            return false;
        }

        public static bool ValidPhoneNumber(string phoneNumber)
        {
            Regex phoneFormat = new Regex(@"^[\d-]+$");

            if (phoneFormat.IsMatch(phoneNumber))
            {
                return true;
            }

            return true;
        }

        public static bool ValidCourseAssessmentCount(int count)
        {
            if (count == 1 || count == 2)
            {
                return true;
            }

            return false;
        }
    }
}
