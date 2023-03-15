using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Degination { get; set; }
        public Guid PartnerId { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Users Partner { get; set; }
    }
}
