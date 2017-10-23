﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class GroupRepository : IGroupRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public GroupRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface Implementation
        public IEnumerable<Group> GetGroups()
        {
            return context.Groups.Include(a => a.Activity).ToList();
        }

        public Group GetGroupByID(int id)
        {
            return context.Groups.Include(a => a.Activity).Where(a => a.GroupID == id).FirstOrDefault();
        }

        public void InsertGroup(Group group)
        {
            context.Groups.Add(group);
        }

        public void DeleteGroup(int id)
        {
            Group group = context.Groups.Find(id);
            context.Groups.Remove(group);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateGroup(Group group)
        {
            context.Entry(group).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        public IEnumerable<Group> GetGroupsAvailable(int clientID)
        {
            List<Group> groups = new List<Group>();
            List<Group> groupsAvailable = context.Groups.ToList();


            foreach (var group in groupsAvailable)
            {
                int id = group.GroupID;

                var registration = context.Registrations
                                          .Where(r => r.ClientID == clientID && r.GroupID == id).FirstOrDefault();

                if (registration == null) groups.Add(group);

            }
            return groups;
        }

        public bool AlumnoGrupo(int clientID, int groupID)
        { 
            var a = context.Registrations.Where(r => r.ClientID == clientID && r.GroupID == groupID).FirstOrDefault();
            if (a == null) 
            {
                return false;
            }
            return true;
        }
        public void AgregarInscripto(int groupID)
        {
            List<Group> groups = context.Groups.ToList();
            var a = context.Groups.Where(r => r.GroupID == groupID).FirstOrDefault();
            if (a != null)
            {
                a.Amount = a.Amount + 1;
            }
                
            
        }
        public void EliminarInscripto(int groupID)
        {
            List<Group> groups = context.Groups.ToList();
            var a = context.Groups.Where(r => r.GroupID == groupID).FirstOrDefault();
            if (a != null)
            {
                a.Amount = a.Amount - 1;
            }
        }
            public void IncrementarCupo (int groupID)
        {
            List<Group> groups = context.Groups.ToList();
            var a = context.Groups.Where(r => r.GroupID == groupID).FirstOrDefault();
            if (a != null)
            {
                a.Quota = a.Quota + 1;
            }
        }
        public void DecrementarCupo(int groupID)
        {
            List<Group> groups = context.Groups.ToList();
            var a = context.Groups.Where(r => r.GroupID == groupID).FirstOrDefault();
            if (a != null)
            {
                a.Quota = a.Quota - 1;
            }
        }

        public bool ValidarCupo(int groupID)
        {
            List<Group> groups = context.Groups.ToList();
            var a = context.Groups.Where(r => r.GroupID == groupID).FirstOrDefault();
            if (a != null && a.Quota==0)
            {
                return false;
            }
            return true;
        }
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