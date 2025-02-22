
namespace RestaurantManagement.Models
{
    public class ReservationInput
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfPeople { get; set; }
    }
}
