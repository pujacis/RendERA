using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class Parameters
    {
        public Guid ParameterId { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
    }
}
