using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class MessageReply
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string ReplyMessage { get; set; }
        public DateTime ReplyDateTime { get; set; }
        public Guid FromId { get; set; }
    }
}
