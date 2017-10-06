using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface ISupplierRepository : IDisposable
    {
        IEnumerable<Supplier> GetSuppliers();
        Supplier GetSupplierByID(int supplierID);
        void InsertSupplier(Supplier supplier);
        void DeleteSupplier(int supplierID);
        void UpdateSupplier(Supplier supplier);
        void Save();
    }
}
