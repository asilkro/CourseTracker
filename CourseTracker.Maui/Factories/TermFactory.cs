using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Factories
{
    internal class TermFactory : FactoryBase<Term>
    {
        public Term? CreateTerm(AddTermsVM addTermsVM, out string errorMessage)
        {
            return CreateTerm(addTermsVM.Term.TermId, addTermsVM.Term.TermName,
                addTermsVM.Term.TermStart, addTermsVM.Term.TermEnd,
                addTermsVM.Term.NotificationsEnabled, addTermsVM.Term.CourseCount,out errorMessage);
        }

        public Term? CreateTerm(int termId, string termName, DateTime termStart, DateTime termEnd,
            bool notificationsEnabled, int courseCount, out string errorMessage)
        {
            if (!IsValidTerm(termId, termName, termStart, termEnd,
                notificationsEnabled, courseCount, out errorMessage))
            {
                return null;
            }

            var term = CreateObject();
            term.TermId = termId;
            term.TermName = termName;
            term.TermStart = termStart;
            term.TermEnd = termEnd;
            return term;
        }

        public bool IsValidTerm(int termId, string termName, DateTime termStart,
            DateTime termEnd, bool notificationsEnabled, int courseCount, out string errorMessage)
        {
            errorMessage = "";

            if (termId <= 0)
                errorMessage = "Term ID must be greater than 0.";
            else if (Validation.IsNull(termName))
                errorMessage = "Term name cannot be empty.";
            else if (termStart < DateTime.MinValue)
                errorMessage = "Term start date cannot be earlier than the minimum value.";
            else if (termEnd < DateTime.MinValue)
                errorMessage = "Term end date cannot be earlier than the minimum value.";
            else if (termStart > DateTime.MaxValue)
                errorMessage = "Term start date cannot be later than the maximum value.";
            else if (termEnd > DateTime.MaxValue)
                errorMessage = "Term end date cannot be later than the maximum value.";
            else if (termStart > termEnd)
                errorMessage = "Term start date cannot be after term end date.";
            else if (!Validation.TermsAreValid(termStart, termEnd))
                errorMessage = "Term start and end dates must be the first and last days of the month, respectively.";
            else if (courseCount < 0)
                errorMessage = "Course count cannot be less than 0.";
            else if (courseCount > 6)
                errorMessage = "Course count cannot be greater than 6.";

            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Term? CreateDefaultObject()
        {
            return new Term();
        }

    }
}
