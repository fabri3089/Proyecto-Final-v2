using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IPaymentTypeRepository : IDisposable
    {
        IEnumerable<PaymentType> GetPaymentTypes();
        PaymentType GetPaymentTypeByID(int paymentTypeID);
        PaymentTypePrice GetCurrentPriceByID(int paymentTypeID);
        Activity GetActivityByPaymentTypeID(int id);
        void InsertPaymentType(PaymentType paymentType);
        void DeletePaymentType(int paymentTypeID);
        void UpdatePaymentType(PaymentType paymentType);
        void Save();
    }
}
