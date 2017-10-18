using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ProyectoFinal.Utils;

namespace ProyectoFinal.Models
{
   /* public class OldGroup
    {
        public int GroupID { get; set; }
        [DisplayName("Nombre")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Descripcion")]
        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayName("Nivel")]
        public Catalog.LevelGroup Level { get; set; }
        [Required]
        [DisplayName("Nivel")]
        public int Level { get; set; }
        [DisplayName("Cupo")]
        [Required]
        public int Quota { get; set; }
        [DisplayName("Cantidad de inscriptos")]
        [Required]
        public int Amount { get; set; }
        [DisplayName("Hora inicio")]
        [RegularExpression("([01]?[0-9]|2[0-3]):[0-5][0-9](:[0-5][0-9])?", ErrorMessage = "Ingrese formato HH:mm.")]
        public string StartTime{ get; set; }
        [DisplayName("Hora fin")]
        [RegularExpression("^((?!City)[a-zA-Z '])+$", ErrorMessage = "Ingrese formato HH:mm.")]
        public string ClosingTime { get; set; } 

       

    }*/
}