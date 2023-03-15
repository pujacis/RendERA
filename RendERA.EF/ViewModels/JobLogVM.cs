using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.DB.ViewModels
{
    public partial class JobLogVM
    {
        public string TrackingId { get; set; }
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int Status { get; set; }
        public string LogDetail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsRead { get; set; }
    }
}
