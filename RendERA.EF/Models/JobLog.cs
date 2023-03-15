using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class JobLog
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int Status { get; set; }
        public string LogDetail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsRead { get; set; }

        public virtual Document Document { get; set; }
    }
}
