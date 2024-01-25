using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Factories
{
    internal class TermFactory : FactoryBase<Term>
    {
        public Term? CreateTerm(AddTermsVM addTermsVM, out string errorMessage)
        {
            return CreateTerm(addTermsVM.Term.TermId, addTermsVM.Term.TermName, addTermsVM.Term.TermStart, addTermsVM.Term.TermEnd, addTermsVM.Term.NotificationsEnabled, addTermsVM.Term.CourseCount,out errorMessage);
        }

        public Term? CreateTerm(int termId, string termName, DateTime termStart, DateTime termEnd, bool notificationsEnabled, int courseCount, out string errorMessage)
        {
            if (!IsValidTerm(termId, termName, termStart, termEnd, notificationsEnabled, courseCount, out errorMessage))
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

        public bool IsValidTerm(int termId, string termName, DateTime termStart, DateTime termEnd, bool notificationsEnabled, int courseCount, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrEmpty(termName))
                errorMessage = "Term name cannot be empty.";
            else if (termStart > termEnd)
                errorMessage = "Term start date cannot be after term end date.";

            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Term? CreateDefaultObject()
        {
            return new Term();
        }


    }
}
