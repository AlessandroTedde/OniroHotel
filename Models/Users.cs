namespace OniroHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Reservations = new HashSet<Reservations>();
        }

        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password obbligatoria.")]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string UserRole { get; set; }

        public short? Fidelity { get; set; }

        [Required(ErrorMessage = "Email obbligatoria.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nome obbligatorio.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Cognome obbligatorio.")]
        [StringLength(50)]
        public string Surname { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        public long? Telephone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservations> Reservations { get; set; }
    }
}
