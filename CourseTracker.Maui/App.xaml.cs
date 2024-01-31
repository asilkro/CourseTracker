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
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
