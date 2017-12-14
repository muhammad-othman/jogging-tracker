using System.ComponentModel.DataAnnotations;

namespace JoggingTrackerServer.Presentation.Models
{
    public class LoginViewModel
    {
        [Required]
        [MinLength(5)]
        public string UserName { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public bool Remmber { get; set; }
    }
}