using System;
using System.ComponentModel.DataAnnotations;

namespace JoggingTrackerServer.Presentation.Models
{
    public class JogViewModel
    {
        public int ID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Distance { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Duration { get; set; }
        [Required]
        public int UserID { get; set; }
        public string UserName { get; set; }

    }
}