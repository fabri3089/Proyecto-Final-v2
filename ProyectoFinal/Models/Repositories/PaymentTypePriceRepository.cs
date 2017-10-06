using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class PaymentTypePriceRepository : IPaymentTypePriceRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public PaymentTypePriceRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<PaymentTypePrice> GetPaymentTypePrices()
        {
            return context.PaymentTypePrices.Include(p => p.PaymentType).ToList();
        }

        public PaymentTypePrice GetPaymentTypePriceByID(int id)
        {
            return context.PaymentTypePrices.Include(p => p.PaymentType).Where(p => p.PaymentTypePriceID == id).FirstOrDefault();
        }

        public void InsertPaymentTypePrice(PaymentTypePrice paymentTypePrice)
        {
            context.PaymentTypePrices.Add(paymentTypePrice);
        }

        public void DeletePaymentTypePrice(int id)
        {
            PaymentTypePrice paymentTypePrice = context.PaymentTypePrices.Find(id);
            context.PaymentTypePrices.Remove(paymentTypePrice);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdatePaymentTypePrice(PaymentTypePrice paymentTypePrice)
        {
            context.Entry(paymentTypePrice).State = EntityState.Modified;
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