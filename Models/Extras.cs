namespace OniroHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Extras
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Extras()
        {
            ReservationDetails = new HashSet<ReservationDetails>();
        }

        [Key]
        public int ExtraID { get; set; }

        [Required]
        [StringLength(50)]
        public string ExtraName { get; set; }

        [Required]
        public string ExtraImg { get; set; }

        public int ExtraPrice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationDetails> ReservationDetails { get; set; }
    }
}
