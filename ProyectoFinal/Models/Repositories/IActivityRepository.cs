using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IActivityRepository : IDisposable
    {
        IEnumerable<Activity> GetActivities();
        Activity GetActivityByID(int activityID);
        void InsertActivity(Activity activity);
        void DeleteActivity(int activityID);
        void UpdateActivity(Activity activity);
        void Save();
    }
}
