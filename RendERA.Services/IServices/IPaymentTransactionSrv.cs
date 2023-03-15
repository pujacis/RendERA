using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface IPaymentTransactionSrv
    {
        void Insert(DB.ViewModels.PaymentTransactionVM model);
        List<DB.ViewModels.PaymentTransactionVM> GetAll();
        DB.ViewModels.PaymentTransactionVM GetByDocId(int docId);
    }
}
