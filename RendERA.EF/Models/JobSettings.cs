using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class JobSettings
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid ParameterId { get; set; }
    }
}
