using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class StockRepository : IStockRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public StockRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<Stock> GetStocks()
        {
            return context.Stocks.ToList();
        }

        public Stock GetStockByID(int id)
        {
            return context.Stocks.Find(id);
        }

        public void InsertStock(Stock stock)
        {
            context.Stocks.Add(stock);
        }

        public void DeleteStock(int id)
        {
            Stock stock = context.Stocks.Find(id);
            context.Stocks.Remove(stock);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateStock(Stock stock)
        {
            context.Entry(stock).State = EntityState.Modified;
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