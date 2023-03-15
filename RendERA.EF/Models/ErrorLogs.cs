using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class ErrorLogs
    {
        public int Id { get; set; }
        public string InnerException { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string HelpLink { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
