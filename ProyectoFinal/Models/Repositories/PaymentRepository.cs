using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class PaymentRepository : IPaymentRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public PaymentRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<Payment> GetPayments()
        {
            return context.Payments.Include(p => p.Client).Include(p => p.PaymentType).ToList();
        }

        public Payment GetPaymentByID(int id)
        {
            return context.Payments.Include(p => p.Client).Include(p => p.PaymentType).Where(p => p.PaymentID == id).FirstOrDefault();
        }

        public IEnumerable<String> GetClientsByActivity(int activityID)
        {
            return context.Payments.Include(p => p.Client)
                                   .Include(p => p.PaymentType)
                                   .Where(c => c.PaymentType.ActivityID == activityID)
                                   .Select(p => p.Client.Email).Distinct()
                                   .ToList();
        }

        public void InsertPayment(Payment payment)
        {
            context.Payments.Add(payment);
        }

        public void DeletePayment(int id)
        {
            Payment payment = context.Payments.Find(id);
            context.Payments.Remove(payment);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdatePayment(Payment payment)
        {
            context.Entry(payment).State = EntityState.Modified;
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