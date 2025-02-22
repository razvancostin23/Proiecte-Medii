using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using RestaurantApp.Views;
using RestaurantApp.Services;
using Microsoft.Maui.Controls.Maps;

namespace RestaurantApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            //  Inițializare notificări pe Windows
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                builder.UseLocalNotification();
                NotificationService.SendNotification(" Sistem activat", "Notificările sunt activate pe Windows.");
            }

            return builder.Build();
            
            
        }
    }
}
