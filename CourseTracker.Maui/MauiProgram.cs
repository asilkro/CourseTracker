﻿using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SQLite;
using Plugin.LocalNotification;
using CourseTracker.Maui.Data;

namespace CourseTracker.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseLocalNotification();
            builder.Services.AddSingleton<TermsDB>();
            builder.Services.AddSingleton<CourseDB>();
            builder.Services.AddSingleton<AssessmentDB>();

#if DEBUG
            builder.Logging.AddDebug();  
#endif
            return builder.Build();
        }
    }
}