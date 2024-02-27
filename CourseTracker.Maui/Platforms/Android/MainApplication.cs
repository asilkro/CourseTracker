using Android;
using Android.App;
using Android.Runtime;

[assembly: UsesPermission(Manifest.Permission.WakeLock)]
//Required so that the plugin can reschedule notifications upon a reboot
[assembly: UsesPermission(Manifest.Permission.ReceiveBootCompleted)]
[assembly: UsesPermission(Manifest.Permission.Vibrate)]
[assembly: UsesPermission("android.permission.POST_NOTIFICATIONS")]

[assembly: UsesPermission("android.permission.USE_EXACT_ALARM")]
[assembly: UsesPermission("android.permission.SCHEDULE_EXACT_ALARM")]
// Taken from documentation here:
// https://github.com/thudugala/Plugin.LocalNotification/wiki/1.-Usage-10.0.0--.Net-MAUI

namespace CourseTracker.Maui.Platforms.Android
{

    [Application]
    public class MainApplication(nint handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }


}
