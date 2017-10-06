using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IClientRepository : IDisposable
    {
        IEnumerable<Client> GetClients();
        Client GetClientByID(int clientID);
        Client GetClientByDocNumber(int docNumber);
        void InsertClient(Client client);
        void DeleteClient(int clientID);
        void UpdateClient(Client client);
        void Save();
        void HashPassword(Client client);
        bool HasActivePayment(Client client);
        bool IsEmailAlreadyInUse(Client client);
        IEnumerable<String> GetClientsWithDebt();
        Dictionary<Activity, bool> ListOfPayments(Client client);
    }
}
