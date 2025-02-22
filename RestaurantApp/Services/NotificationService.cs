using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotification;
using Microsoft.Maui.Devices;

namespace RestaurantApp.Services
{
    public static class NotificationService
    {
        public static void SendNotification(string title, string message)
        {
            // Notificare trimisă doar dacă rulează pe Windows
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                var request = new NotificationRequest
                {
                    NotificationId = 100,
                    Title = !string.IsNullOrEmpty(title) ? title : " Programare Restaurant",
                    Subtitle = " Notificare Programare",
                    Description = !string.IsNullOrEmpty(message) ? message : "O nouă programare a fost adăugată cu succes!",
                    BadgeNumber = 1,
                    ReturningData = "ProgramareAdaugata",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(2)  // Notificare după 2 secunde
                    },
                    Windows = new Plugin.LocalNotification.WindowsOption.WindowsOptions { }                    
                };
                LocalNotificationCenter.Current.Show(request);
            }
        }
    }
}
