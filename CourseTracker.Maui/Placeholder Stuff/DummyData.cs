using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Maui.Placeholder_Stuff;
public class DummyData
{

    public List<String> CourseNames = new List<string>
    {
        "Course Name Placeholder",
        "Course Name Placeholder",
        "Course Name Placeholder",
        "Course Name Placeholder"
    };

    public List<String> InstructorNames = new List<string>
    {
        "Instructor Name Placeholder",
        "Instructor Name Placeholder",
        "Instructor Name Placeholder",
        "Instructor Name Placeholder"
    };

    public List<String> InstructorEmails = new List<string>
    {
        "instructor@contoso.edu",
        "instructor@contoso.edu",
        "instructor@contoso.edu",
        "instructor@contoso.edu"
    };

    public List<String> InstructorPhones = new List<string>
    {
        "553-555-9052",
        "552-555-1892",
        "551-555-2189",
        "554-555-7556"
    };

    public List<String> TermNames = new List<string>
    {
        "The First Term",
        "Some Second Term",
        "Terrible Third Term",
        "Fantastic Fourth Term"
    };

    public List<String> CourseStatuses = new List<string>
    {
        "Planned",
        "Planned",
        "Completed",
        "In Progress"
    };

    public List<String> AssessmentNames = new List<string>
    {
        "OA 123",
        "PA 555",
        "PA 988",
        "OA 789"
    };


    //DateTime CourseStart1 = DateTime.Now.Date.AddDays(-320);
    //DateTime CourseStart2 = DateTime.Now.Date.AddDays(-190);
    //DateTime CourseStart3 = DateTime.Now.Date.AddDays(-210);
    //DateTime CourseStart4 = DateTime.Now.Date.AddDays(-60);

    //DateTime CourseEnd1 = CourseStart1.Date.AddDays(90);
    //DateTime CourseEnd2 = CourseStart2.Date.AddDays(140);
    //DateTime CourseEnd3 = CourseStart3.Date.AddDays(120);
    //DateTime CourseEnd4 = CourseStart4.Date.AddDays(45);
}


