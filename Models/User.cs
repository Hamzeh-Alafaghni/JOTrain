using System.ComponentModel.DataAnnotations;

namespace JOTrain.Models
{
    public class User
    {
        [Key] //primary key
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Client;
    }
}
