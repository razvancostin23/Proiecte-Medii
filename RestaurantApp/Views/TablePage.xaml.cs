using RestaurantApp.Models;

namespace RestaurantApp.Views
{
    public partial class TablePage : ContentPage
    {
        public TablePage()
        {
            InitializeComponent();
            _ = LoadTables();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTables();
        }

        private async Task LoadTables()
        {
            tableListView.ItemsSource = await App.Database.GetTablesAsync();
        }

        private async void OnAddTableClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TableDetailPage());
        }

        private async void OnTableSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Table selectedTable)
            {
                bool edit = await DisplayAlert("Editare/Ștergere",
                    $"Vrei să editezi sau să ștergi masa {selectedTable.TableNumber}?",
                    "Editează", "Șterge");

                if (edit)
                {
                    await Navigation.PushAsync(new TableDetailPage(selectedTable));
                }
                else
                {
                    bool confirm = await DisplayAlert("Confirmare", "Sigur ștergi această masă?", "Da", "Nu");
                    if (confirm)
                    {
                        await App.Database.DeleteTableAsync(selectedTable);
                        await LoadTables();
                    }
                }
            }
        }
    }
}
