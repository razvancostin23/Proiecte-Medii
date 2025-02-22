using SQLite;
using RestaurantApp.Models;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace RestaurantApp.Data
{
    public class DatabaseContext
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseContext(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Table>().Wait();
            _database.CreateTableAsync<Reservation>().Wait();
            _database.CreateTableAsync<RestaurantMenuItem>().Wait();
            _database.CreateTableAsync<Order>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }

        // CRUD pentru tabelul Table

        // Citire (Read) - obține toate mesele
        public Task<List<Table>> GetTablesAsync() => _database.Table<Table>().ToListAsync();

        // Salvare (Create/Update) - adaugă sau actualizează o masă
        public Task<int> SaveTableAsync(Table table) =>
            table.Id != 0 ? _database.UpdateAsync(table) : _database.InsertAsync(table);

        // Ștergere (Delete) - șterge o masă
        public Task<int> DeleteTableAsync(Table table) => _database.DeleteAsync(table);

        // Obține o masă după ID (pentru detalii individuale)
        public Task<Table> GetTableByIdAsync(int id) =>
            _database.Table<Table>().FirstOrDefaultAsync(t => t.Id == id);

        // CRUD pentru Reservation
        public Task<List<Reservation>> GetReservationsAsync() => _database.Table<Reservation>().ToListAsync();

        public Task<int> SaveReservationAsync(Reservation reservation) =>
            reservation.Id != 0 ? _database.UpdateAsync(reservation) : _database.InsertAsync(reservation);

        public Task<int> DeleteReservationAsync(Reservation reservation) => _database.DeleteAsync(reservation);

        public Task<Reservation> GetReservationByIdAsync(int id) =>
            _database.Table<Reservation>().FirstOrDefaultAsync(r => r.Id == id);

        // CRUD pentru RestaurantMenuItem
        public Task<List<RestaurantMenuItem>> GetMenuItemsAsync() => _database.Table<RestaurantMenuItem>().ToListAsync();

        public Task<int> SaveMenuItemAsync(RestaurantMenuItem menuItem) =>
            menuItem.Id != 0 ? _database.UpdateAsync(menuItem) : _database.InsertAsync(menuItem);

        public Task<int> DeleteMenuItemAsync(RestaurantMenuItem menuItem) => _database.DeleteAsync(menuItem);

        public Task<RestaurantMenuItem> GetMenuItemByIdAsync(int id) =>
            _database.Table<RestaurantMenuItem>().FirstOrDefaultAsync(m => m.Id == id);

        // CRUD pentru Order
        public Task<List<Order>> GetOrdersAsync() => _database.Table<Order>().ToListAsync();

        public Task<int> SaveOrderAsync(Order order) =>
            order.Id != 0 ? _database.UpdateAsync(order) : _database.InsertAsync(order);

        public Task<int> DeleteOrderAsync(Order order) => _database.DeleteAsync(order);

        public Task<Order> GetOrderByIdAsync(int id) =>
            _database.Table<Order>().FirstOrDefaultAsync(o => o.Id == id);

        // CRUD pentru User
        public Task<User> GetUserAsync(string username, string password) =>
            _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

        public Task<User> GetUserByUsernameAsync(string username) =>
            _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username);

        public Task<int> SaveUserAsync(User user) =>
            user.Id != 0 ? _database.UpdateAsync(user) : _database.InsertAsync(user);
    }
}
