using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class SupplierRepository : ISupplierRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public SupplierRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<Supplier> GetSuppliers()
        {
            return context.Suppliers.ToList();
        }

        public Supplier GetSupplierByID(int id)
        {
            return context.Suppliers.Find(id);
        }

        public void InsertSupplier(Supplier supplier)
        {
            context.Suppliers.Add(supplier);
        }

        public void DeleteSupplier(int id)
        {
            Supplier supplier = context.Suppliers.Find(id);
            context.Suppliers.Remove(supplier);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            context.Entry(supplier).State = EntityState.Modified;
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