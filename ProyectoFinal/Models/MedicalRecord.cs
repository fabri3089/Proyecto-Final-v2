using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class MedicalRecord
    {
        public int MedicalRecordID { get; set; }

        [Required]
        [DisplayName("Sexo")]
        public char Gender { get; set; }

        [Required]
        [DisplayName("Altura")]
        public double Weight { get; set; }

        [Required]
        [DisplayName("Peso")]
        public double Heigth { get; set; }

        [Required]
        [DisplayName("Edad")]
        public int Age { get; set; }

        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; }
        public int ClientID { get; set; }

    }
}