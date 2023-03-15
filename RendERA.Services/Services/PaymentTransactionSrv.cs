using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using RendERA.ServiceManager.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.Services
{
    public class PaymentTransactionSrv : IPaymentTransactionSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public PaymentTransactionSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public List<PaymentTransactionVM> GetAll()
        {
            var query = from t in _unitOfWork.IPaymentTransactionRepo.Table.OrderByDescending(a => a.CreatedDate)
                        select new PaymentTransactionVM
                        {
                            Id = t.Id,
                            CouponCode = t.CouponCode,
                            CouponId = t.CouponId,
                            DocumentId = t.DocumentId,
                            PayableAmount = t.PayableAmount,
                            PaymentId = t.PaymentId,
                            PercentOff = t.PercentOff,
                            ReturnStatus = t.ReturnStatus,
                            Status = t.Status,
                            TotalAmount = t.TotalAmount,
                            TransactionId = t.TransactionId,
                            CreatedDate = DateTime.Now
                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }
        
        public PaymentTransactionVM GetByDocId(int docId)
        {
            var model = _unitOfWork.IPaymentTransactionRepo.Table.Where(a => a.DocumentId == docId && a.Status == "COMPLETED").FirstOrDefault();
            if (model != null)
            {
                var vm = new PaymentTransactionVM()
                {
                    Id = model.Id,
                    CouponCode = model.CouponCode,
                    CouponId = model.CouponId,
                    DocumentId = model.DocumentId,
                    PayableAmount = model.PayableAmount,
                    PaymentId = model.PaymentId,
                    PercentOff = model.PercentOff,
                    ReturnStatus = model.ReturnStatus,
                    Status = model.Status,
                    TotalAmount = model.TotalAmount,
                    TransactionId = model.TransactionId,
                    CreatedDate = DateTime.Now
                };
                return vm;
            }
            return null;
        }

        public void Insert(PaymentTransactionVM model)
        {
            if (model != null)
            {
                var m = new PaymentTransaction()
                {
                    CouponCode =model.CouponCode,
                    CouponId = model.CouponId,
                   DocumentId = model.DocumentId,
                   PayableAmount =model.PayableAmount,
                   PaymentId =model.PaymentId,
                   PercentOff = model.PercentOff,
                   ReturnStatus =model.ReturnStatus,
                   Status = model.Status,
                   TotalAmount = model.TotalAmount,
                   TransactionId =model.TransactionId,
                   CreatedDate = DateTime.Now
                };
                _unitOfWork.IPaymentTransactionRepo.Add(m);
                _unitOfWork.IPaymentTransactionRepo.Save();
            }
        }
    }
}
