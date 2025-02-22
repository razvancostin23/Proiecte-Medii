using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class OrderPage : ContentPage
    {
        public OrderPage()
        {
            InitializeComponent();
            _ = LoadOrders();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadOrders();
        }

        private async Task LoadOrders()
        {
            orderListView.ItemsSource = await App.Database.GetOrdersAsync();
        }

        private async void OnAddOrderClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderDetailPage());
        }

        private async void OnOrderSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Order selectedOrder)
            {
                bool edit = await DisplayAlert("Editare/Ștergere",
                    $"Vrei să editezi sau să ștergi comanda pentru rezervarea {selectedOrder.ReservationId}?",
                    "Editează", "Șterge");

                if (edit)
                {
                    await Navigation.PushAsync(new OrderDetailPage(selectedOrder));
                }
                else
                {
                    bool confirm = await DisplayAlert("Confirmare", "Sigur ștergi această comandă?", "Da", "Nu");
                    if (confirm)
                    {
                        await App.Database.DeleteOrderAsync(selectedOrder);
                        await LoadOrders();
                    }
                }
            }
        }
    }
}
