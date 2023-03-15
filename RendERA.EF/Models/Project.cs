using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class Project
    {
        public Project()
        {
            Document = new HashSet<Document>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public Guid PartnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Users Partner { get; set; }
        public virtual ICollection<Document> Document { get; set; }
    }
}
