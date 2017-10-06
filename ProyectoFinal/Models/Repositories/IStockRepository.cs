using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IStockRepository : IDisposable
    {
        IEnumerable<Stock> GetStocks();
        Stock GetStockByID(int stockID);
        void InsertStock(Stock stock);
        void DeleteStock(int stockID);
        void UpdateStock(Stock stock);
        void Save();
    }
}
