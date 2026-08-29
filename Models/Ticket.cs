using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JOTrain.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int TripId { get; set; }
        
        public Trip? Trip { get; set; }

        public int UserId { get; set; }
        
        public User? User { get; set; }

        public SeatClass ClassType { get; set; }
        public PaymentMethod PaymentVia { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}