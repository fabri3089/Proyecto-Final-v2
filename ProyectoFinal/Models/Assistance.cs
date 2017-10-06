using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Assistance
    {
        public int AssistanceID { get; set; }
        [Required]
        [DisplayName("Fecha asistencia")]
        public DateTime assistanceDate { get; set; }
        public int ClientID { get; set; }
        

    }
}