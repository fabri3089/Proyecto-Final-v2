using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IPaymentRepository : IDisposable
    {
        IEnumerable<Payment> GetPayments();
        Payment GetPaymentByID(int paymentID);
        void InsertPayment(Payment payment);
        void DeletePayment(int paymentID);
        void UpdatePayment(Payment payment);
        void Save();
        IEnumerable<String> GetClientsByActivity(int activityID);
    }
}
