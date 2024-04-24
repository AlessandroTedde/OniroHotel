namespace OniroHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reservations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservations()
        {
            ReservationDetails = new HashSet<ReservationDetails>();
        }

        [Key]
        public int ResID { get; set; }

        public int UserID { get; set; }

        [Column(TypeName = "date")]
        public DateTime InDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime OutDate { get; set; }

        [Required]
        [StringLength(2)]
        public string Treatment { get; set; }

        public int Total { get; set; }

        public bool IsValid { get; set; }

        public int Guests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationDetails> ReservationDetails { get; set; }

        public virtual Users Users { get; set; }
    }
}
