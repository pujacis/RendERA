using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class Document
    {
        public Document()
        {
            JobLog = new HashSet<JobLog>();
            ParameterDocumnetMapping = new HashSet<ParameterDocumnetMapping>();
            PaymentTransaction = new HashSet<PaymentTransaction>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUploadUrl { get; set; }
        public DateTime DatePosted { get; set; }
        public Guid? AssignedPartnerId { get; set; }
        public Guid UploadedBy { get; set; }
        public int? MessageId { get; set; }
        public string FileDownloadUrl { get; set; }
        public bool IsParameter { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AssignedTeamId { get; set; }
        public bool? IsSubmitted { get; set; }
        public int? AssignedProjectId { get; set; }
        public string TransactionId { get; set; }
        public string TrackingId { get; set; }

        public virtual Project AssignedProject { get; set; }
        public virtual ICollection<JobLog> JobLog { get; set; }
        public virtual ICollection<ParameterDocumnetMapping> ParameterDocumnetMapping { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransaction { get; set; }
    }
}
