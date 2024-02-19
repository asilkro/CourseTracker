using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Data;
public class SharedDB
{
    #region Fields
    ViewModelBase vmb;
    public TermsDB termsDB;
    public CourseDB courseDB;
    public AssessmentDB assessmentDB;
    Connection connection;
    #endregion

    #region Constructor
    public SharedDB()
    {
        termsDB = new TermsDB();
        courseDB = new CourseDB();
        assessmentDB = new AssessmentDB();
        connection = new Connection();
        vmb = new ViewModelBase();
    }
    #endregion

    #region Utility Methods 

    //This could probably be moved to another
    //class but its here for convenience right now.
    public async Task<bool> ConfirmedAction(string messageRequiringConfirmation)
    {

        bool result = await Application.Current.MainPage.DisplayAlert("Confirm Action", messageRequiringConfirmation, "Yes", "No");
        return result;
    }
    #endregion
    #region Term Methods
    public async Task InsertTerm(Term newTerm)
    {
        try
        {
            await termsDB.SaveTermAsync(newTerm);
            vmb.ShowToast("Database entry for Term " + newTerm.TermName + " successfully modified.");
            return;
        }
        catch (Exception e)
        {
            vmb.ShowToast("Error adding term: " + e.Message);
            return;
        }
    }
    public async Task DeleteTermAndRelatedEntities(Term term)
    {
        var confirmed = await ConfirmedAction("Are you sure you want to delete " + term.TermName +
            "and ALL " + term.CourseCount + " of its associated courses and assessments?" +
            "\r\nThis action cannot be undone.");
        if (!confirmed) { return; }
        {
            var con = connection.GetConnection();
            try
            {
                con.BeginTransaction();
                List<Course> courses = con.Table<Course>().Where(c => c.TermId == term.TermId).ToList();
                foreach (var course in courses)
                {
                    await DeleteCourseAndRelatedEntities(course, showConfirmation: false);
                }
                await termsDB.DeleteTermAsync(term);
                con.Commit();
            }
            catch (Exception e)
            {
                vmb.ShowToast("Error deleting term " + term.TermName + ": " + e.Message);
                throw;
            }
            finally
            {
                con.Dispose();
            }
            return;
        }
    }
    #endregion

    #region Course Methods
    public async Task InsertCourseAndUpdateTerm(Course newCourse)
    {
        var term = await termsDB.GetTermByIdAsync(newCourse.TermId);
        if (term == null)
        {
            vmb.ShowToast("Term not found.");
            return;
        }

        if (term.CourseCount >= 6)
        {
            vmb.ShowToast("A term cannot have more than 6 courses.");
            return;
        }
        var tx = connection.GetConnection();
        try
        {
            using (tx)
            {
                tx.BeginTransaction();
                await courseDB.SaveCourseAsync(newCourse);
                term.CourseCount += 1;
                await termsDB.SaveTermAsync(term);
                tx.Commit();
            }
        }
        catch (Exception e)
        {
            vmb.ShowToast("Error adding course: " + e.Message);
            return;
        }
        finally
        {
            tx.Dispose();
            vmb.ShowToast("Term " + term.TermName + " updated successfully.");
            vmb.ShowToast("Course " + newCourse.CourseName + " added successfully.");
        }
        return;
    }
    public async Task DeleteCourseAndRelatedEntities(Course course, bool showConfirmation = true)
    {
        bool confirmed = true;
        if (showConfirmation)
        {
            confirmed = await ConfirmedAction("Are you sure you want to delete this course and ALL " +
            course.CourseAssessmentCount + " of its associated assessments?" + "\r\nThis action cannot be undone.");
        }
        if (confirmed)
        {
            try
            {
                var con = connection.GetConnection();
                try
                {
                    con.BeginTransaction();
                    var assessments = con.Table<Assessment>().Where(a => a.RelatedCourseId == course.CourseId).ToList();
                    foreach (var assessment in assessments)
                    {
                        await assessmentDB.DeleteAssessmentAsync(assessment);

                    }
                    await courseDB.DeleteCourseAsync(course);
                    con.Commit();
                }
                catch (Exception e)
                {
                    con.Rollback();
                    vmb.ShowToast("Error deleting course " + course.CourseName + ": " + e.Message);
                }
                finally { con.Dispose(); }
            }
            finally
            {
                vmb.ShowToast("Course " + course.CourseName + " deleted successfully.");
            }
        }
        else
        {
            return;
        }
    }
    #endregion
}