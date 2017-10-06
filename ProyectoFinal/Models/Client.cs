using ProyectoFinal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class Client
    {
        public int ClientID { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Apellido")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Tipo Doc.")]
        public string DocType { get; set; }

        [Required]
        [DisplayName("Nro. Doc.")]
        public int DocNumber { get; set; }

        [DisplayName("Fecha nacimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required]
        [DisplayName("Dia inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateFrom { get; set; }

        [EmailAddress]
        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Sexo")]
        [Range(1, 2, ErrorMessage = "Elija un género")]
        public Catalog.Genre Sexo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Editable(false)]
        public string PasswordSalt { get; set; }

        [Required]
        [DisplayName("Rol")]
        [Range(1, 3, ErrorMessage = "Elija uno de los roles disponibles")]
        public Catalog.Roles Role { get; set; }

        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<Routine> Routines { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}