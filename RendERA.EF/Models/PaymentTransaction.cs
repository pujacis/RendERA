using System;
using System.Collections.Generic;

namespace RendERA.DB.Models
{
    public partial class PaymentTransaction
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int? CouponId { get; set; }
        public string CouponCode { get; set; }
        public decimal? PercentOff { get; set; }
        public decimal PayableAmount { get; set; }
        public string ReturnStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Document Document { get; set; }
    }
}
