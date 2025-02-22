using RestaurantApp.Data;
using RestaurantApp.Views;
using System.IO;

namespace RestaurantApp
{
    public partial class App : Application
    {
        static DatabaseContext? database;

        public static DatabaseContext Database
        {
            get
            {
                if (database == null)
                {
                    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "restaurant.db3");
                    database = new DatabaseContext(dbPath);
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());  // Pagina de login la pornire
        }
    }
}
