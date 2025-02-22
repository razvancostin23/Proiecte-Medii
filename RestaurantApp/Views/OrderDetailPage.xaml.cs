using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class OrderDetailPage : ContentPage
    {
        private Order? currentOrder;

        public OrderDetailPage(Order? order = null)
        {
            InitializeComponent();
            currentOrder = order;
            if (currentOrder != null)
            {
                reservationIdEntry.Text = currentOrder.ReservationId.ToString();
                orderDatePicker.Date = currentOrder.OrderDate;
                orderTimePicker.Time = currentOrder.OrderDate.TimeOfDay;
                totalAmountEntry.Text = currentOrder.TotalAmount.ToString();
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!int.TryParse(reservationIdEntry.Text, out int reservationId))
            {
                await DisplayAlert("Eroare", "ID-ul rezervării trebuie să fie un număr valid.", "OK");
                return;
            }

            var reservation = await App.Database.GetReservationByIdAsync(reservationId);
            if (reservation == null)
            {
                await DisplayAlert("Eroare", "Rezervarea specificată nu există.", "OK");
                return;
            }

            if (!decimal.TryParse(totalAmountEntry.Text, out decimal totalAmount) || totalAmount <= 0)
            {
                await DisplayAlert("Eroare", "Suma totală trebuie să fie un număr pozitiv.", "OK");
                return;
            }

            if (currentOrder == null)
                currentOrder = new Order();

            currentOrder.ReservationId = reservationId;
            currentOrder.OrderDate = orderDatePicker.Date + orderTimePicker.Time;
            currentOrder.TotalAmount = totalAmount;

            await App.Database.SaveOrderAsync(currentOrder);
            await Navigation.PopAsync();
        }

    }
}
