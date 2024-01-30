using Microsoft.Extensions.DependencyInjection;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Views;
using Microsoft.Maui.Platform;
using System;

namespace CourseTracker.Maui
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        NavigationPage navigation;
        
        public App()
        {
            /*InitializeComponent();

            navigation = new NavigationPage(new MainPage());
            NavigationPage.SetHasNavigationBar(navigation, true);

            MainPage = navigation;*/ //-- This is the original code

            var services = new ServiceCollection();

            //Services and dependencies being registered
            //Database
            //services.AddSingleton<IAsyncSqLite>(); // Interface
            services.AddSingleton<Connection>(); // Class that implements the interface

            //Factories
            services.AddSingleton<CourseFactory>();
            services.AddSingleton<TermFactory>();
            services.AddSingleton<AssessmentFactory>();
            services.AddSingleton<InstructorFactory>();

            //Pages being registered
            services.AddTransient<Homepage>();
            services.AddTransient<MainPage>();
            //Courses
            services.AddTransient<ListCourses>();
            services.AddTransient<AddCourses>();
            services.AddTransient<EditCourses>();
            //Terms
            services.AddTransient<ListTerms>();
            services.AddTransient<AddTerms>();
            services.AddTransient<EditTerms>();

            _serviceProvider = services.BuildServiceProvider();

        }

        protected override void OnStart()
        {
            MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MainPage>());
            var homepage = _serviceProvider.GetRequiredService<Homepage>();
            var navigationPage = new NavigationPage(homepage);

            var listCoursesPage = _serviceProvider.GetRequiredService<ListCourses>();
            var listTermsPage = _serviceProvider.GetRequiredService<ListTerms>();
            var addCoursesPage = _serviceProvider.GetRequiredService<AddCourses>();
            var editCoursesPage = _serviceProvider.GetRequiredService<EditCourses>();
            var addTermsPage = _serviceProvider.GetRequiredService<AddTerms>();
            var editTermsPage = _serviceProvider.GetRequiredService<EditTerms>();

            var navigationPageCourses = new NavigationPage(listCoursesPage);
            var navigationPageTerms = new NavigationPage(listTermsPage);
            var navigationPageAddCourses = new NavigationPage(addCoursesPage);
            var navigationPageEditCourses = new NavigationPage(editCoursesPage);
            var navigationPageAddTerms = new NavigationPage(addTermsPage);
            var navigationPageEditTerms = new NavigationPage(editTermsPage);

        }

    }
}
