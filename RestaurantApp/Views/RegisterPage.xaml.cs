using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage() => InitializeComponent();

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameEntry.Text) || string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                await DisplayAlert("Eroare", "Completează toate câmpurile!", "OK");
                return;
            }

            if (passwordEntry.Text != confirmPasswordEntry.Text)
            {
                await DisplayAlert("Eroare", "Parolele nu coincid!", "OK");
                return;
            }

            var existingUser = await App.Database.GetUserByUsernameAsync(usernameEntry.Text);
            if (existingUser != null)
            {
                await DisplayAlert("Eroare", "Numele de utilizator este deja folosit.", "OK");
                return;
            }

            var user = new User { Username = usernameEntry.Text, Password = passwordEntry.Text };
            await App.Database.SaveUserAsync(user);
            await DisplayAlert("Succes", "Cont creat cu succes!", "OK");
            await Navigation.PopAsync();
        }

        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
