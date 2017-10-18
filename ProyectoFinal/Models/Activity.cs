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
            this.ActivitySchedules = new HashSet<Group>();
        }

        public int ActivityID { get; set; }
        [DisplayName("Nombre")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Descripción")]
        [Required]
        public string Description { get; set; }
        
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Group> ActivitySchedules { get; set; }
        public virtual ICollection<PaymentType> PaymentTypes { get; set; }
    }
}