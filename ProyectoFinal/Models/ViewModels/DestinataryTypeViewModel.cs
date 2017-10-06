using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class DestinataryTypeViewModel
    {
        [Required()]
        public int ID { get; set; }
        [Required()]
        public String Type { get; set; }
    }
}