using SQLite;

namespace RestaurantApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string? Username { get; set; }

        public string? Password { get; set; }  
    }
}
