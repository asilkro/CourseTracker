using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.ViewModels;
using SQLite;

namespace CourseTracker.Maui.Factories
{
    internal class TermFactory : FactoryBase<Term>
    {
       public TermFactory(IAsyncSqLite database) : base(database)
        {
        }

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

        public Term? UpdateTerm(EditTermsVM editTermsVM, out string errorMessage)
        {
            return UpdateTerm(editTermsVM.Term.TermId, editTermsVM.Term.TermName,
                editTermsVM.Term.TermStart, editTermsVM.Term.TermEnd,
                editTermsVM.Term.NotificationsEnabled, editTermsVM.Term.CourseCount, out errorMessage);
        }

        public Term? UpdateTerm(int termId, string termName, DateTime termStart, DateTime termEnd,
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

            if (!Validation.IdWasSet(termId))
                errorMessage = "Term ID must be greater than 0.";
            else if (!Validation.NotNull(termName))
                errorMessage = "Term name cannot be empty.";
            else if (!Validation.TermsAreValid(termStart, termEnd))
                errorMessage = "Term start and end dates must be the first and last days of the month, respectively.";
            else if (!Validation.CourseCountIsValid(courseCount))
                errorMessage = "Terms must have between 1 and 6 courses.";
            else if (!Validation.DatesAreValid(termStart, termEnd))
                errorMessage = "Term start and end dates are not valid.";

            Debug.WriteLine(errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Term? CreateDefaultObject()
        {
            return new Term();
        }

    }
}
