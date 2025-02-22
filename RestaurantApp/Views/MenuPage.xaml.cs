using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            _ = LoadMenuItems();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadMenuItems();
        }

        private async Task LoadMenuItems()
        {
            menuListView.ItemsSource = await App.Database.GetMenuItemsAsync();
        }

        private async void OnAddMenuItemClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuItemDetailPage());
        }

        private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is RestaurantMenuItem selectedItem)
            {
                bool edit = await DisplayAlert("Editare/Ștergere",
                    $"Vrei să editezi sau să ștergi produsul {selectedItem.Name}?",
                    "Editează", "Șterge");

                if (edit)
                {
                    await Navigation.PushAsync(new MenuItemDetailPage(selectedItem));
                }
                else
                {
                    bool confirm = await DisplayAlert("Confirmare", "Sigur ștergi acest produs?", "Da", "Nu");
                    if (confirm)
                    {
                        await App.Database.DeleteMenuItemAsync(selectedItem);
                        await LoadMenuItems();
                    }
                }
            }
        }
    }
}
