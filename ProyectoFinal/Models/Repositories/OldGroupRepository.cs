using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{ }

   /* public class OldGroupRepository: IOldGroupRepository, IDisposable
    {
       /* #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public OldGroupRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<OldGroup> GetGroups()
        {
            return context.Groups.ToList();
        }

        public OldGroup GetGroupByID(int id)
        {
            return context.Groups.Find(id);
        }

        public IEnumerable<OldGroup> GetGroupsAvailable(int clientID)
        {
            List<OldGroup> groups = new List<OldGroup>();
            List<OldGroup> groupsAvailable = context.Groups.ToList();


            foreach (var group in groupsAvailable)
            {
                int id = group.GroupID;

                var registration = context.Registrations
                                          .Where(r => r.ClientID == clientID && r.GroupID == id).FirstOrDefault();

                if (registration == null) groups.Add(group);
                
            }
            return groups;
        }

        public void InsertGroup(OldGroup group)
        {
            context.Groups.Add(group);
        }

        public void DeleteGroup(int id)
        {
            OldGroup group = context.Groups.Find(id);
            context.Groups.Remove(group);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateGroup(OldGroup Group)
        {
            context.Entry(Group).State = EntityState.Modified;
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
}*/
