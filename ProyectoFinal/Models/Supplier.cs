using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        [Required]
        [DisplayName("Razón social")]
        public string BusinessName { get; set; }

        [EmailAddress]
        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Teléfono")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Dirección")]
        public string Address { get; set; }

        [Required]
        [DisplayName("Código postal")]
        public int PostalCode { get; set; }

        [Required]
        [DisplayName("Ciudad")]
        public string City { get; set; }

        [Required]
        [DisplayName("País")]
        public string Country { get; set; }

        [Url]
        [DisplayName("Sitio web")]
        public string WebSite { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}