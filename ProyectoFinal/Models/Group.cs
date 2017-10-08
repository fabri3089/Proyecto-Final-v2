using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        [DisplayName("Nombre")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Descripcion")]
        [Required]
        public string Description { get; set; }
        [DisplayName("Nivel")]
        [Required]
        public int Level { get; set; }
        [DisplayName("Cupo")]
        [Required]
        public int Quota { get; set; }
        [DisplayName("Cantidad de inscriptos")]
        [Required]
        public int Amount { get; set; }

    }
}