using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class Jobs
    {
        public Guid JobId { get; set; }
        public Guid UserId { get; set; }
        public Guid? PartnerId { get; set; }
        public string FileUploadUrl { get; set; }
        public string FileDownloadUrl { get; set; }
        public short JobStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Remark { get; set; }
    }
}
