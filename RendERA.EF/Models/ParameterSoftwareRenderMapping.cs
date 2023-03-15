using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class ParameterSoftwareRenderMapping
    {
        public int ParameterId { get; set; }
        public int SoftwareRenderId { get; set; }
    }
}
