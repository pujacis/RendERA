using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class ParameterCostvalues
    {
        public int Id { get; set; }
        public decimal RenderingPerSecondPrice { get; set; }
        public decimal RenderingPerNodePrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
