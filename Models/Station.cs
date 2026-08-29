using System.ComponentModel.DataAnnotations;

namespace JOTrain.Models
{
    public class Station
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}