using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Activity
    {
        public Activity()
        {
            this.Clients = new HashSet<Client>();
            this.ActivitySchedules = new HashSet<ActivitySchedule>();
        }

        public int ActivityID { get; set; }
        [DisplayName("Nombre")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Descripción")]
        [Required]
        public string Description { get; set; }
        
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<ActivitySchedule> ActivitySchedules { get; set; }
        public virtual ICollection<PaymentType> PaymentTypes { get; set; }
    }
}