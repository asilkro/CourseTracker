using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Data;
public class SharedDB
{
    #region Fields
    public TermsDB termsDB;
    public CourseDB courseDB;
    public AssessmentDB assessmentDB;
    //Connection connection;
    #endregion

    #region Constructor
    public SharedDB()
    {
        termsDB = new TermsDB();
        courseDB = new CourseDB();
        assessmentDB = new AssessmentDB();
        //connection = new Connection();
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
    public async Task SaveTerm(Term newTerm)
    {
        try
        {
            await termsDB.SaveTermAsync(newTerm);
            ShowToast("Database entry for Term " + newTerm.TermName + " successfully modified.");
            return;
        }
        catch (Exception e)
        {
            ShowToast("Error adding term: " + e.Message);
            return;
        }
    }

    public async Task<int> DeleteTermAsync(Term term)
    {
        int count = term.CourseCount;
        var result = await Application.Current.MainPage.DisplayAlert(
            "WARNING: DELETING TERM", "Are you SURE you want to delete this term?" +
            " It will also remove " + count + " courses." +
            "If you wish to keep this data, please move the existing courses to another term first.", "Yes", "No");
        if (result)
        {
            List<Course> allCourses = await courseDB.GetCoursesAsync();
            foreach (Course course in allCourses)
            {
                if (course.TermId == term.TermId)
                {
                    await courseDB.DeleteCourseAsync(course);
                }
            }

            return await termsDB.DeleteTermAsync(term);
        }
        return 0;
    }
    public async Task DeleteTermAndRelatedEntities(Term term)
    {
        var confirmed = await ConfirmedAction("Are you sure you want to delete " + term.TermName +
            "and ALL " + term.CourseCount + " of its associated courses and assessments?" +
            "\r\nThis action cannot be undone.");
        if (!confirmed) { return; }
        else
        {
            //var con = connection.GetConnection();
            try
            {
                //con.BeginTransaction();

                List<Course> courses = await courseDB.GetCoursesByTermIdAsync(term.TermId);
                foreach (var course in courses)
                {
                    await DeleteCourseAndRelatedEntities(course, showConfirmation: false);
                }
                await termsDB.DeleteTermAsync(term);
                //con.Commit();
            }
            catch (Exception e)
            {
                ShowToast("Error deleting term " + term.TermName + ": " + e.Message);
                //con.Rollback();
                throw;
            }
            finally
            {
                //con.Dispose();
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
            ShowToast("Term not found.");
            return;
        }

        if (term.CourseCount >= 6)
        {
            ShowToast("A term cannot have more than 6 courses.");
            return;
        }
        //var tx = connection.GetConnection();
        try
        {
            //using (tx)
            {
                //tx.BeginTransaction();
                await courseDB.SaveCourseAsync(newCourse);
                term.CourseCount += 1;
                await termsDB.SaveTermAsync(term);
                //tx.Commit();
            }
        }
        catch (Exception e)
        {
            ShowToast("Error adding course: " + e.Message);
            //tx.Rollback();
            return;
        }
        finally
        {
            // tx.Dispose();
            ShowToast("Term " + term.TermName + " updated successfully.");
            ShowToast("Course " + newCourse.CourseName + " added successfully.");
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
                //var con = connection.GetConnection();
                try
                {
                    //con.BeginTransaction();
                    var assessments = await assessmentDB.GetAssessmentsByCourseIdAsync(course.CourseId);
                    foreach (var assessment in assessments)
                    {
                        await assessmentDB.DeleteAssessmentAsync(assessment);

                    }
                    await courseDB.DeleteCourseAsync(course);
                    //con.Commit();
                }
                catch (Exception e)
                {
                    //con.Rollback();
                    ShowToast("Error deleting course " + course.CourseName + ": " + e.Message);
                }
                finally
                {
                    //con.Dispose();
                }
            }
            finally
            {
                ShowToast("Course " + course.CourseName + " deleted successfully.");
            }
        }
        else
        {
            return;
        }
    }
    #endregion

    #region Assessment Methods
    public async Task SaveAssessmentAndUpdateCourse(Assessment newAssessment)
    {
        var course = await courseDB.GetCourseByIdAsync(newAssessment.RelatedCourseId);
        if (course == null)
        {
            ShowToast("Course not found.");
            return;
        }

        if (course.CourseAssessmentCount >= 2)
        {
            ShowToast("Courses may have no more than two assessments.");
            return;
        }
        try
        {
            {
                await assessmentDB.SaveAssessmentAsync(newAssessment);
                
                course.CourseAssessmentCount += 1;
                
                await courseDB.SaveCourseAsync(course);
            }
        }
        catch (Exception e)
        {
            ShowToast("Error adding assessment: " + e.Message);
            return;
        }
        finally
        {
            ShowToast("Course " + course.CourseName + " updated successfully.");
            ShowToast("Assessment added successfully.");
        }
        return;
    }

    private async void ShowToast(string message)
    {
        CancellationTokenSource cancellationTokenSource = new();

        string text = message;
        ToastDuration duration = ToastDuration.Short;
        double fontSize = 14;

        var toast = Toast.Make(text, duration, fontSize);

        await toast.Show(cancellationTokenSource.Token);
    }
    #endregion
}