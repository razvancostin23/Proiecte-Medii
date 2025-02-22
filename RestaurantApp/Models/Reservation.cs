using SQLite;

namespace RestaurantApp.Models
{
    public class Reservation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public int TableId { get; set; }
    }
}
