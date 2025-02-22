using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage() => InitializeComponent();

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var user = await App.Database.GetUserAsync(usernameEntry.Text, passwordEntry.Text);
            if (user != null)
            {
                Application.Current!.MainPage = new AppShell();  // Navigare către aplicație
            }
            else
            {
                await DisplayAlert("Eroare", "Utilizator sau parolă incorectă!", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
