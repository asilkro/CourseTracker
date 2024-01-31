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
        //private IServiceProvider _serviceProvider;
        NavigationPage navigation;
        
        public App()
        {
            InitializeComponent();
            NavigationPage navigationPage = new(new MainPage());
            navigation = navigationPage;
            NavigationPage.SetHasNavigationBar(navigation, true);

            MainPage = navigation; //-- This is the original code

        }
      
    }
}
