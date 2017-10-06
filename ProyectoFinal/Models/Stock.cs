using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Stock
    {
        public int StockID { get; set; }

        [DisplayName("Artículo")]
        public int ArticleID { get; set; }


        [Editable(false)]
        [DisplayName("Stock")]
        public int CantInStock { get; set; }

        [DisplayName("Stock deseado")]
        public int DesiredStock { get; set; }
    }
}