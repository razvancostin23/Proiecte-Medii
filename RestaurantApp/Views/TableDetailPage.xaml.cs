using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class TableDetailPage : ContentPage
    {
        private Table? currentTable;

        public TableDetailPage(Table? table = null)
        {
            InitializeComponent();
            currentTable = table;
            if (currentTable != null)
            {
                tableNumberEntry.Text = currentTable.TableNumber.ToString();
                seatsEntry.Text = currentTable.Seats.ToString();
                statusPicker.SelectedItem = currentTable.Status;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tableNumberEntry.Text) ||
                string.IsNullOrWhiteSpace(seatsEntry.Text) ||
                statusPicker.SelectedItem == null)
            {
                await DisplayAlert("Eroare", "Completează toate câmpurile!", "OK");
                return;
            }

            if (currentTable == null)
            {
                currentTable = new Table();
            }

            currentTable.TableNumber = int.Parse(tableNumberEntry.Text);
            currentTable.Seats = int.Parse(seatsEntry.Text);
            currentTable.Status = statusPicker.SelectedItem.ToString();

            await App.Database.SaveTableAsync(currentTable);
            await Navigation.PopAsync();
        }
    }
}
