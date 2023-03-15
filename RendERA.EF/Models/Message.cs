using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string MessageToPost { get; set; }
        public Guid To { get; set; }
        public Guid From { get; set; }
        public DateTime DatePosted { get; set; }
        public Guid? JobUploadedBy { get; set; }
    }
}
