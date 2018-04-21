using System;
using System.Collections.Generic;

namespace DojoActivityCenter.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<RSVP> Events { get; set; }

        public User()
        {
            this.CreatedAt = DateTime.Now;
            this.Events = new List<RSVP>();
        }

        public User(Register user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.Password = user.Password;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Events = new List<RSVP>();
        }

    }
}