using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IPaymentTypePriceRepository : IDisposable
    {
        IEnumerable<PaymentTypePrice> GetPaymentTypePrices();
        PaymentTypePrice GetPaymentTypePriceByID(int paymentTypePriceID);
        void InsertPaymentTypePrice(PaymentTypePrice paymentTypePrice);
        void DeletePaymentTypePrice(int paymentTypePriceID);
        void UpdatePaymentTypePrice(PaymentTypePrice paymentTypePrice);
        void Save();
    }
}
