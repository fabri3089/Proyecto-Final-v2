using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class PaymentTypeRepository : IPaymentTypeRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public PaymentTypeRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<PaymentType> GetPaymentTypes()
        {
            return context.PaymentTypes.Include(p => p.Activity)
                                       .Include(p => p.PaymentTypePrices)
                                       .ToList();
        }

        public PaymentType GetPaymentTypeByID(int id)
        {
            return context.PaymentTypes.Include(p => p.Activity)
                                       .Include(p => p.PaymentTypePrices)
                                       .Where(p => p.PaymentTypeID == id)
                                       .FirstOrDefault();
        }

        public Activity GetActivityByPaymentTypeID(int id)
        {
            return context.PaymentTypes.Include(p => p.Activity)
                                       .Where(p => p.PaymentTypeID == id)
                                       .Select(p => p.Activity)
                                       .FirstOrDefault();
        }

        public PaymentTypePrice GetCurrentPriceByID(int paymentTypeID)
        {
            var prices = context.PaymentTypePrices.Where(p => p.PaymentTypeID == paymentTypeID).ToList();
            return prices.OrderByDescending(p => p.DateFrom).FirstOrDefault();
        }

        public void InsertPaymentType(PaymentType paymentType)
        {
            context.PaymentTypes.Add(paymentType);
        }

        public void DeletePaymentType(int id)
        {
            PaymentType paymentType = context.PaymentTypes.Find(id);
            context.PaymentTypes.Remove(paymentType);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdatePaymentType(PaymentType paymentType)
        {
            context.Entry(paymentType).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}