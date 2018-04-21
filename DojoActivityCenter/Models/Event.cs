using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DojoActivityCenter.Models
{
    public class Event : BaseEntity
    {
        public int EventId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        [MinLength(2)]
        public string Title { get; set; }                

        [Required]
        [MinLength(10)]
        public string Description { get; set; }                

        [Required(ErrorMessage = "Please enter a valid date.")]
        [CurrentDate(ErrorMessage = "The scheduled event must be on or after today's date.")]
        [Display(Name="Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter a valid time.")]
        [Display(Name="Time")]
        public TimeSpan Time { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<RSVP> Guests { get; set; }

        public Event()
        {
            this.CreatedAt = DateTime.Now;

            this.Guests = new List<RSVP>();
        }
    }
}