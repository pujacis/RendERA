using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class ServerList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
