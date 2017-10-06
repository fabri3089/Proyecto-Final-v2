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
    public class PaymentType
    {
        public int PaymentTypeID { get; set; }

        [Required]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Estado")]
        public Catalog.Status Status { get; set; }

        [Required]
        [DisplayName("Duración")]
        public int DurationInMonths { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PaymentTypePrice> PaymentTypePrices { get; set; }

        [ForeignKey("ActivityID")]
        public virtual Activity Activity { get; set; }
        public int ActivityID { get; set; }
    }
}