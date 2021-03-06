﻿using ProyectoFinal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        [DisplayName("Nombre Clase")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Descripcion")]
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Nivel")]
        public Catalog.LevelGroup Level { get; set; }
        [DisplayName("Cupo")]
        [Required]
        public int Quota { get; set; }
        [DisplayName("Cantidad de inscriptos")]
        [Required]
        public int Amount { get; set; }
        [DisplayName("Día")]
        [Required]
        public string Day { get; set; }
        [DisplayName("Hora desde")]
        [Required]
        public float HourFrom { get; set; }
        [Required]
        [DisplayName("Hora hasta")]
        public float HourTo { get; set; }

        [ForeignKey("ActivityID")]
        public Activity Activity { get; set; }
        public int ActivityID { get; set; }
    }
}