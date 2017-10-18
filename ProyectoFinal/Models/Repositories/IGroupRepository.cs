using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IGroupRepository : IDisposable
    {
        IEnumerable<Group> GetGroups();
        Group GetGroupByID(int groupID);
        void InsertGroup(Group group);
        void DeleteGroup(int groupID);
        void UpdateGroup(Group group);
        void Save();
        IEnumerable<Group> GetGroupsAvailable(int clientID);
    }
}
