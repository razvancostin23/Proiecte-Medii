using RestaurantApp.Views;

namespace RestaurantApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Înregistrare rute pentru navigare
            Routing.RegisterRoute(nameof(Views.TableDetailPage), typeof(Views.TableDetailPage));
            Routing.RegisterRoute(nameof(Views.ReservationDetailPage), typeof(Views.ReservationDetailPage));
            Routing.RegisterRoute(nameof(Views.MenuItemDetailPage), typeof(Views.MenuItemDetailPage));
            Routing.RegisterRoute(nameof(Views.OrderDetailPage), typeof(Views.OrderDetailPage));
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            // Navigare către pagina de login și resetare sesiune
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
