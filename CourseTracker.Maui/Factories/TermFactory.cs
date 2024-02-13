using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Factories
{
    public class TermFactory : FactoryBase<Term>
    {

        public TermFactory(IAsyncSqLite database) : base (database)
        {

        }

        public Term? CreateTerm(TermVM termVM, out string errorMessage)
        {
            return CreateTerm(termVM.Term.TermId, termVM.Term.TermName,
                termVM.Term.TermStart, termVM.Term.TermEnd,
                termVM.Term.CourseCount,out errorMessage);
        }

        public Term? CreateTerm(int termId, string termName, DateTime termStart, DateTime termEnd,
            int courseCount, out string errorMessage)
        {
            if (!IsValidTerm(termId, termName, termStart, termEnd,
                courseCount, out errorMessage))
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

        public Term? UpdateTerm(TermVM termVM, out string errorMessage)
        {
            return UpdateTerm(termVM.Term.TermId, termVM.Term.TermName,
                termVM.Term.TermStart, termVM.Term.TermEnd,
                termVM.Term.CourseCount, out errorMessage);
        }

        public Term? UpdateTerm(int termId, string termName, DateTime termStart, DateTime termEnd,
            int courseCount, out string errorMessage)
        {
            if (!IsValidTerm(termId, termName, termStart, termEnd,
                courseCount, out errorMessage))
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
            DateTime termEnd, int courseCount, out string errorMessage)
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
            else if (Validation.IsUniqueTermName(termName, new()).Result == false)
                errorMessage = "Term name must be unique.";

            Debug.WriteLine(errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Term? CreateDefaultObject()
        {
            return new Term();
        }

    }
}
