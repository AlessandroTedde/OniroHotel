namespace OniroHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Applications
    {
        [Key]
        public int ApplicationID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Cognome")]
        public string Surname { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Telefono")]
        public int? Phone { get; set; }

        [Required]
        [Display(Name = "File Inserito")]
        public string InsertedFile { get; set; }
    }
}
