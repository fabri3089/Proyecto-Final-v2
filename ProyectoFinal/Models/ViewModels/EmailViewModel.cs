using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.ViewModels
{
    public class EmailViewModel
    {
        private String _title;
        private String _subtitle;
        private String _innerTitle;
        private String _description;

        [Required()]
        [MaxLength(100,ErrorMessage = "La longitud máxima del título es de 100 caracteres")]
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }
        [Required()]
        [MaxLength(100, ErrorMessage = "La longitud máxima del subtítulo es de 100 caracteres")]
        public String SubTitle
        {
            get { return _subtitle; }
            set { _subtitle = value; }
        }
        [Required()]
        [MaxLength(100, ErrorMessage = "La longitud máxima del asunto es de 100 caracteres")]
        public String InnerTitle
        {
            get { return _innerTitle; }
            set { _innerTitle = value; }
        }
        [Required()]
        [MaxLength(1000, ErrorMessage = "La longitud máxima de la descripción es de 1000 caracteres")]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

    }
}