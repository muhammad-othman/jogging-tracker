using JoggingTrackerServer.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JoggingTrackerServer.Presentation.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }
        [Required]
        [MinLength(5)]
        public string UserName { get; set; }
        [Required]
        [Range(10, 100)]
        public int Age { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        public UserPermission Permission { get; set; }
    }
}