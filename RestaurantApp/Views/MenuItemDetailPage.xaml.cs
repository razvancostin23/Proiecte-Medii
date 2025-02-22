using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class MenuItemDetailPage : ContentPage
    {
        private RestaurantMenuItem? currentMenuItem;

        public MenuItemDetailPage(RestaurantMenuItem? menuItem = null)
        {
            InitializeComponent();
            currentMenuItem = menuItem;
            if (currentMenuItem != null)
            {
                nameEntry.Text = currentMenuItem.Name;
                descriptionEntry.Text = currentMenuItem.Description;
                priceEntry.Text = currentMenuItem.Price.ToString();
                categoryPicker.SelectedItem = currentMenuItem.Category;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameEntry.Text) || nameEntry.Text.Length < 3)
            {
                await DisplayAlert("Eroare", "Numele produsului trebuie să aibă cel puțin 3 caractere.", "OK");
                return;
            }

            if (!decimal.TryParse(priceEntry.Text, out decimal price) || price <= 0)
            {
                await DisplayAlert("Eroare", "Prețul trebuie să fie un număr pozitiv.", "OK");
                return;
            }

            if (categoryPicker.SelectedItem == null)
            {
                await DisplayAlert("Eroare", "Selectează o categorie pentru produs.", "OK");
                return;
            }

            if (currentMenuItem == null)
                currentMenuItem = new RestaurantMenuItem();

            currentMenuItem.Name = nameEntry.Text;
            currentMenuItem.Description = descriptionEntry.Text;
            currentMenuItem.Price = price;
            currentMenuItem.Category = categoryPicker.SelectedItem.ToString();

            await App.Database.SaveMenuItemAsync(currentMenuItem);
            await Navigation.PopAsync();
        }

    }
}
