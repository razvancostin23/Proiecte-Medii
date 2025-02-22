using RestaurantApp.Models;
using RestaurantApp.Services;

namespace RestaurantApp.Views
{
    public partial class ReservationDetailPage : ContentPage
    {
        private Reservation? currentReservation;

        public ReservationDetailPage(Reservation? reservation = null)
        {
            InitializeComponent();
            currentReservation = reservation;
            if (currentReservation != null)
            {
                customerNameEntry.Text = currentReservation.CustomerName;
                phoneEntry.Text = currentReservation.PhoneNumber;
                reservationDatePicker.Date = currentReservation.ReservationDate;
                reservationTimePicker.Time = currentReservation.ReservationDate.TimeOfDay;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(customerNameEntry.Text) || customerNameEntry.Text.Length < 3)
            {
                await DisplayAlert("Eroare", "Numele clientului trebuie să aibă cel puțin 3 caractere.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(phoneEntry.Text) || phoneEntry.Text.Length < 10)
            {
                await DisplayAlert("Eroare", "Numărul de telefon trebuie să fie valid.", "OK");
                return;
            }

            if (reservationDatePicker.Date < DateTime.Today)
            {
                await DisplayAlert("Eroare", "Data rezervării nu poate fi în trecut.", "OK");
                return;
            }

            if (currentReservation == null)
                currentReservation = new Reservation();

            currentReservation.CustomerName = customerNameEntry.Text;
            currentReservation.PhoneNumber = phoneEntry.Text;
            currentReservation.ReservationDate = reservationDatePicker.Date + reservationTimePicker.Time;

            await App.Database.SaveReservationAsync(currentReservation);
            // Trimitere notificare după salvare
            NotificationService.SendNotification(
                " Programare confirmată",
                $"Programarea pentru {currentReservation.CustomerName} a fost adăugată cu succes!"
            );
            await Navigation.PopAsync();
        }

    }
}
