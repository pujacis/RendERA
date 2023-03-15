using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class ConfigurationSettings
    {
        public int Id { get; set; }
        public int Pro { get; set; }
        public int Expert { get; set; }
        public int Beginner { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
