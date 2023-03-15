using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class Users
    {
        public Users()
        {
            Project = new HashSet<Project>();
            Team = new HashSet<Team>();
        }

        public Guid UserId { get; set; }
        public short UserType { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public bool IsVerified { get; set; }
        public bool? ReadWelcomePopup { get; set; }
        public int? UserCategory { get; set; }

        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<Team> Team { get; set; }
    }
}
