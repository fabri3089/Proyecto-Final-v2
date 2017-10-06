using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Inscripcion
    {
        public int InscripcionID { get; set; }

        public DateTime CreationDate { get; set; }

        public string Status { get; set; }

        [ForeignKey("ClientID")]
        [DisplayName("Cliente")]
        public virtual Client Client { get; set; }
        public int ClientID { get; set; }

        [ForeignKey("GroupID")]
        [DisplayName("Grupo")]
        public virtual Group Group { get; set; }
        public int GroupID { get; set; }
    }
}