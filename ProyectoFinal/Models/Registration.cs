using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Registration
    {
        public int RegistrationID { get; set; }
        [DisplayName("Fecha de alta")]
        public DateTime CreationDate { get; set; }
        [DisplayName("Estado")]
        public string Status { get; set; }

        [DisplayName("Cliente")]
        [ForeignKey("ClientID")]
        public Client Client { get; set; }
        [DisplayName("Cliente")]
        public int ClientID { get; set; }

        [DisplayName("Grupo")]
        [ForeignKey("GroupID")]
        public  Group Group { get; set; }
        [DisplayName("Grupo")]
        public int GroupID { get; set; }
    }
}