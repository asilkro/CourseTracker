namespace CourseTracker.Maui
{
    public partial class App : Application
    {
        NavigationPage navigation;
        
        public App()
        {
            InitializeComponent();

            navigation = new NavigationPage(new MainPage());
            NavigationPage.SetHasNavigationBar(navigation, true);

            MainPage = navigation;

        }
    }
}
