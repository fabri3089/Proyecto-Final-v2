using ProyectoFinal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class File
    {
        public int FileID { get; set; }

        [Required]
        [DisplayName("Músculo")]
        public string MuscleName { get; set; }

        [Required]
        [DisplayName("Ejercicio")]
        public string ExerciseName { get; set; }

        [Required]
        [DisplayName("Peso")]
        public string Peso { get; set; }

        [Required]
        [DisplayName("Repeticiones")]
        public string Repetitions { get; set; }

        [Required]
        [DisplayName("Día")]
        public string Day { get; set; }

        [ForeignKey("RoutineID")]
        public Routine Routine { get; set; }
        public int RoutineID { get; set; }
    }
}