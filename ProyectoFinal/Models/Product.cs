using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static ProyectoFinal.Utils.Catalog;

namespace ProyectoFinal.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Tipo")]
        public ProductType Type { get; set; }

        [Required]
        [DisplayName("Precio")]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Estado")]
        public ProductStatus Status { get; set; }

        [DisplayName("Fecha compra")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Stock")]
        public int UnitsInStock { get; set; }

        [DisplayName("Unidades pedidas")]
        public int UnitsInOrder { get; set; }

        [ForeignKey("SupplierID")]
        public Supplier Supplier { get; set; }
        public int SupplierID { get; set; }
    }
}