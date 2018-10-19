﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication9.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class contactos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public contactos()
        {
            this.contacto_empresa = new HashSet<contacto_empresa>();
            this.usuario = new HashSet<usuario>();

        }

        [Display(Name = "Id de Contacto")]
        [Required(ErrorMessage = "Debe seleccionar id")]
        public int id { get; set; }
        [Required(ErrorMessage = "Debe seleccionar rut")]
        [Display(Name = "RUT")]
        public string rut { get; set; }
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Debe seleccionar Nombre")]
        public string nombres { get; set; }
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Debe seleccionar Apellidos")]
        public string apellidos { get; set; }
        [Display(Name = "Celular Personal")]
        [Required(ErrorMessage = "Debe ingresar número de telefono celular")]
        [DataType(DataType.Text)]
        public int tcel { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Debe seleccionar Email")]
        public string email { get; set; }
        [Display(Name = "Comentario")]
        public string comentario { get; set; }
        [Display(Name = "Curriculum")]
        public string curriculum { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contacto_empresa> contacto_empresa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usuario> usuario { get; set; }

        public string FullName
        {
            get
            {
                return nombres + " " + apellidos;
            }
        }

    }
}