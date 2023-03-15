using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class ParameterDocumnetMapping
    {
        public int DocumentId { get; set; }
        public int ParameterId { get; set; }
        public string Price { get; set; }
        public int Frequency { get; set; }

        public virtual Document Document { get; set; }
        public virtual Parameter Parameter { get; set; }
    }
}
