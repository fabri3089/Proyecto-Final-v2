using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class ActivitySchedule
    {
        public int ActivityScheduleID { get; set; }
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