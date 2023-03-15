using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class Parameter
    {
        public Parameter()
        {
            ParameterDocumnetMapping = new HashSet<ParameterDocumnetMapping>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Category { get; set; }
        public string Value { get; set; }

        public virtual ICollection<ParameterDocumnetMapping> ParameterDocumnetMapping { get; set; }
    }
}
