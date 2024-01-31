using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui.ViewModels
{
    public class MenuItemViewModel
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
    }

    public class MenuViewModel
    {
        CourseFactory _courseFactory;
        TermFactory _termFactory;
        public ObservableCollection<MenuGroupViewModel> MenuItems { get; } = new ObservableCollection<MenuGroupViewModel>();

        public MenuViewModel()
        {
            // Example: Adding menu items to different groups
            var termsGroup = new MenuGroupViewModel { GroupTitle = "Terms" };
            termsGroup.Items.Add(new MenuItemViewModel { Text = "List Terms", Command = new Command(OpenTermsList) });
            termsGroup.Items.Add(new MenuItemViewModel { Text = "Add Terms", Command = new Command(OpenAddTerms) });
            MenuItems.Add(termsGroup);

            var coursesGroup = new MenuGroupViewModel { GroupTitle = "Courses" };
            coursesGroup.Items.Add(new MenuItemViewModel { Text = "List Courses", Command = new Command(OpenTermsList) });
            coursesGroup.Items.Add(new MenuItemViewModel { Text = "Add Courses", Command = new Command(OpenAddCourses) });
            MenuItems.Add(coursesGroup);
        }

        // Example command methods
        private void OpenTermsList() 
        {
            var page = new ListTerms(_termFactory);
        }
        private void OpenAddTerms() 
        {
            var page = new AddTerms(_termFactory);
        }
        private void OpenCoursesList()
        { 
            var page = new ListCourses(_courseFactory);
        }
        private void OpenAddCourses() 
        {
            var page = new AddCourses(_courseFactory);
        }
    }

    public class MenuGroupViewModel
    {
        public string GroupTitle { get; set; }
        public ObservableCollection<MenuItemViewModel> Items { get; } = new ObservableCollection<MenuItemViewModel>();
    }
}
