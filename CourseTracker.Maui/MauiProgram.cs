using CommunityToolkit.Maui;
using CourseTracker.Maui.Data;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

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
                fonts.AddFont(
                    "OpenSans-Regular.ttf",
                    "OpenSansRegular");
                fonts.AddFont(
                    "OpenSans-Semibold.ttf",
                    "OpenSansSemibold");
            })
            .UseLocalNotification();

            builder.Services.AddSingleton<TermsDB>();
            builder.Services.AddSingleton<CourseDB>();
            builder.Services.AddSingleton<AssessmentDB>();
            builder.Services.AddSingleton<SharedDB>();
            builder.Services.AddSingleton<NotifyDB>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}