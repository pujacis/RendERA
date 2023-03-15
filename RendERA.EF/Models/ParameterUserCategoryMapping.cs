using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class ParameterUserCategoryMapping
    {
        public int ParameterId { get; set; }
        public int UserCategoryId { get; set; }
    }
}
