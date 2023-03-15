using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    class PaymentTransactionRepo : GenericRepo<DB.Models.PaymentTransaction>, IRepositories.IPaymentTransactionRepo
    {
        public PaymentTransactionRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }
    }
}
