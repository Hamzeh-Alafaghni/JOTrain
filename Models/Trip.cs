using System.ComponentModel.DataAnnotations;

namespace JOTrain.Models
{
    public class Trip
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a departure station.")]
        public int DepartureStationId { get; set; }
        public Station? DepartureStation { get; set; }

        [Required(ErrorMessage = "Please select an arrival station.")]
        public int ArrivalStationId { get; set; }
        public Station? ArrivalStation { get; set; }

        [Required(ErrorMessage = "Departure time is required.")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Range(0.50, 20.00, ErrorMessage = "Economy price must be between 0.50 and 200 JOD.")]
        public decimal PriceEconomy { get; set; }

        [Required]
        [Range(1.00, 30.00, ErrorMessage = "Economy+ price must be between 1.00 and 300 JOD.")]
        public decimal PriceEconomyPlus { get; set; }

        [Required]
        [Range(2.00, 50.00, ErrorMessage = "VIP price must be between 2.00 and 500 JOD.")]
        public decimal PriceVIP { get; set; }
    }
}