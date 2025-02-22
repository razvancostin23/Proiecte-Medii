using Plugin.LocalNotification;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.ObjectModel;

namespace RestaurantApp.Views
{
    public partial class ReservationPage : ContentPage
    {
        private ObservableCollection<Reservation> reservations;

        public ReservationPage()
        {
            InitializeComponent();
            reservations = new ObservableCollection<Reservation>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadReservationsAsync();  // Încărcare rezervări la deschiderea paginii
        }

        private async Task LoadReservationsAsync()
        {
            var reservationList = await App.Database.GetReservationsAsync();
            reservations.Clear();
            foreach (var reservation in reservationList)
            {
                reservations.Add(reservation);
            }
            reservationListView.ItemsSource = reservations;
        }

        //  Adăugare rezervare nouă
        private async void OnAddReservationClicked(object sender, EventArgs e)
        {
            var reservationDetailPage = new ReservationDetailPage();
            reservationDetailPage.Disappearing += async (s, args) => await LoadReservationsAsync();  // Refresh după adăugare
            await Navigation.PushAsync(reservationDetailPage);
        }

        //  Selectare rezervare cu opțiuni "Editare" și "Ștergere"
        private async void OnReservationSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Reservation selectedReservation)
            {
                string action = await DisplayActionSheet(
                    "Alege o acțiune:", "Anulează", null, "Editare", "Ștergere");

                if (action == "Editare")
                {
                    var detailPage = new ReservationDetailPage(selectedReservation);
                    detailPage.Disappearing += async (s, args) => await LoadReservationsAsync();
                    await Navigation.PushAsync(detailPage);
                }
                else if (action == "Ștergere")
                {
                    bool confirmDelete = await DisplayAlert(
                        "Confirmare", "Sigur dorești să ștergi această rezervare?", "Da", "Nu");
                    if (confirmDelete)
                    {
                        await App.Database.DeleteReservationAsync(selectedReservation);
                        reservations.Remove(selectedReservation);
                    }
                }
                ((ListView)sender).SelectedItem = null;  // Resetează selecția
            }
        }

        
    }
}
