using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class ChangePassViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password antigua")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Nueva password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Repita nueva password")]
        public string PasswordCheck { get; set; }
    }
}