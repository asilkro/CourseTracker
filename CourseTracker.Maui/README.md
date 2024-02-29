# C971 Mobile Application Development Using C#

## Course Tracker by Andrew Silkroski

Thank you for evaluating and using this application. There are a few things you should know about this application before use.

### Evaluator-Specific Notes
**Please use the data setup button on the app's main screen to load the required evaluation data.**

- Please load the evaluation data before adding other data.
- If you have already added data, you will need to reset the database by using the button on the home page.
- When prompted, please allow the application to access your device's notifications as this is required for proper functionality. If you do not allow this, the application will be unable to send you reminders for upcoming assessments and courses.
    - *If you do not receive a "Notifications Working" notification after doing this, please close and reopen the application and try again.*

## Important User Information

1. The application is designed to be used on a mobile device and will work best on hardware. It relies on Android version 12.1 and above.

2. When prompted, please allow the application to access your device's notifications as this is required for proper functionality. If you do not allow this, the application will be unable to send you reminders for upcoming assessments and courses.
 - *If you do not receive a "Notifications Working" notification after doing this, please close and reopen the application and try again.*

3.	If the application is run through an Android emulator, you are likely to encounter issues with the pages containing ListViews not populating properly. This is a known bug/issue with MAUI inside Visual Studio 2022 and it's interaction with the Android Emulator and **not the application or code itself.**
	 
	- To work around this issue, you can run the application on a physical device **(recommended)**, or open the XAML file (ie ListTerms.xaml) and make a small change *(changing CachingStrategy="RecycleElement" to "RetainElement" as an example)** and then save the file.
	- The ListView should display correctly after.

4. There are [known issues with collections](https://github.com/dotnet/maui/issues/19357) and [bindings](https://github.com/dotnet/maui/issues/20002) in MAUI that may cause the Lists to not populate properly. This is an issue with the MAUI framework and not the application or code itself.

## Application Workflow Overview:

1. Add a new term.
	1. A term supports a maximum of 6 courses.
1. Add a new course.
	1. While creating the course, assign it the term via the dropdown.
		1. A course supports a maximum of 2 assessments.
		1. A course supports alerting on 3 and 1 days before the start and end dates.
		1. Course notes are required for sharing functionality.
1. Add a new assessment.
	1. While creating the assessment, assign it to the appropriate course via the dropdown.
		1. An assessment supports alerting on 3 and 1 days before the start and end(due) dates.
	
#### Deletion Notes
- When removing a term, all courses and assessments associated with that term will be removed as well.
	
- When removing a course, all assessments associated with that course will be removed as well.

## NuGet Packages

	- CommunityToolkit.Maui 7.0.1
	- Microsoft.Extensions.Logging.Debug 8.0.0
	- Microsoft.Maui.Controls 8.0.7
	- Microsoft.Maui.Controls.Compatibility 8.0.7
	- Plugin.LocalNotification 11.1.1
	- SQLite-net-pcl 1.8.116
	- SQLitePCLRaw.provider.dynamic_cdecl 2.1.8

## Support
For evaluator questions, please reach out to me via [email](mailto:asilkro@wgu.edu?subject=C971%20Evaluation%20Question)

## Project status
Version 1.0 complete.

No further development planned at this time.

## Acknowledgments
- Application code by [Andrew Silkroski](https://github.com/asilkro).
- Owl app icon by [Andrew Silkroski](https://www.silkroski.com).
- Thank you to my WGU course mentors for their guidance and support.
- **An extra special thank you to my friends, family and loved ones for their patience and understanding.**

## Add your files

- [ ] [Create](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#create-a-file) or [upload](https://docs.gitlab.com/ee/user/project/repository/web_editor.html#upload-a-file) files
- [ ] [Add files using the command line](https://docs.gitlab.com/ee/gitlab-basics/add-file.html#add-a-file-using-the-command-line) or push an existing Git repository with the following command:

```
cd existing_repo
git remote add origin https://gitlab.com/wgu-gitlab-environment/task-templates/c971-mobile-application-development-using-c-sharp.git
git branch -M main
git push -uf origin main
```
