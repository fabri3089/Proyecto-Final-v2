using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class DestinatariesViewModel
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public Utils.Catalog.Destinataries DestinataryType { get; set; }

    }
}