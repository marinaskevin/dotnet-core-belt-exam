using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bright_Ideas.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [InverseProperty("User")]
        public List<Like> Likes { get; set; }
        [InverseProperty("User")]
        public List<Idea> Ideas { get; set; }

        public User()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Likes = new List<Like>();
            this.Ideas = new List<Idea>();
        }

        public User(Register user)
        {
            this.Name = user.Name;
            this.Alias = user.Alias;
            this.Email = user.Email;
            this.Password = user.Password;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Likes = new List<Like>();
            this.Ideas = new List<Idea>();
        }

    }
}