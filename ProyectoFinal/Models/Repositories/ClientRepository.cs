using ProyectoFinal.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ProyectoFinal.Models.Repositories
{
    public class ClientRepository : IClientRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public ClientRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface Implementation
        public IEnumerable<Client> GetClients()
        {
            return context.Clients.ToList();
        }

        public Client GetClientByID(int id)
        {
            return context.Clients.Find(id);
        }

        public void InsertClient(Client client)
        {
            context.Clients.Add(client);
        }

        public void DeleteClient(int id)
        {
            Client client = context.Clients.Find(id);
            context.Clients.Remove(client);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateClient(Client client)
        {
            context.Entry(client).State = EntityState.Modified;
        }

        public void HashPassword(Client client)
        {
            client.PasswordSalt = PasswordUtilities.CreateSalt(16);
            client.Password = PasswordUtilities.GenerateSHA256Hash(client.Password, client.PasswordSalt);
        }

        public Client GetClientByDocNumber(int docNumber)
        {
            return context.Clients.Where(c => c.DocNumber == docNumber).First();
        }

        public Dictionary<Activity, bool> ListOfPayments(Client client)
        {
            Dictionary<Activity, bool> response = new Dictionary<Activity, bool>();
            List<Payment> payments = context.Clients
                                .Include(c => c.Payments)
                                .ToList()
                                .Where(c => c.ClientID == client.ClientID).FirstOrDefault()
                                .Payments.ToList();
            foreach (var payment in payments)
            {
                PaymentTypeRepository paymentTypeRepository = new PaymentTypeRepository(new GymContext());
                Activity activity = paymentTypeRepository.GetActivityByPaymentTypeID(payment.PaymentTypeID);

                if (payment.Status == Catalog.Status.Active && payment.ExpirationDate.Date >= DateTime.Now.Date)
                {
                    response.Add(activity, true);
                }
                else
                {
                    response.Add(activity, false);
                }
            }
            return response;
        }

        public bool HasActivePayment(Client client)
        {
            List<Payment> payments = context.Clients
                                            .Include(c => c.Payments)
                                            .ToList()
                                            .Where(c => c.ClientID == client.ClientID).FirstOrDefault()
                                            .Payments.ToList();
            foreach (var payment in payments)
            {
                if (payment.Status == Catalog.Status.Active && payment.ExpirationDate.Date >= DateTime.Now.Date)
                {
                    return true;
                } 
            }
            return false;
        }

        public IEnumerable<String> GetClientsWithDebt()
        {
            return context.Clients.ToList()
                                  .Where(c => this.HasActivePayment(c) == false && c.Role == Catalog.Roles.Client)
                                  .Select(c => c.Email)
                                  .Distinct().ToList();
        }

        public bool IsEmailAlreadyInUse(Client client)
        {
            StackTrace stackTrace = new StackTrace();
            bool isEditTheCaller = stackTrace.GetFrame(1).GetMethod().Name.Equals("Edit");

            if (isEditTheCaller) //Si viene de Edit, debo permitirle guardar el email que ya tenía anteriormente
            {
                return context.Clients
                                      .Where(c => c.ClientID != client.ClientID)
                                      .Any(c => c.Email.ToLower() == client.Email.ToLower());
            }
            else
            {
                return context.Clients
                                       .Any(c => c.Email.ToLower() == client.Email.ToLower());
            }
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