using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class DefaultRenderSoftwares
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int SoftwareEnumId { get; set; }
    }
}
