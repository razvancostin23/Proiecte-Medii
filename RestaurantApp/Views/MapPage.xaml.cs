using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;

namespace RestaurantApp.Views
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            LoadMap();
        }

        private async void LoadMap()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync()
                               ?? await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

                if (location != null)
                {
                    RestaurantMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Location(location.Latitude, location.Longitude),
                        Distance.FromKilometers(1)
                    ));
                }
                else
                {
                    await DisplayAlert("Atenție", "Nu s-a putut obține locația.", "OK");
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Eroare", "Geolocalizarea nu este suportată pe acest dispozitiv.", "OK");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Eroare", "Permisiunea pentru locație a fost refuzată.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare neașteptată", $"Detalii: {ex.Message}", "OK");
            }
        }
    }
}
