namespace OniroHotel.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Nome Extra")]
        public string ExtraName { get; set; }

        [Required]
        [Display(Name = "Immagine")]
        public string ExtraImg { get; set; }
        [Display(Name = "Prezzo")]
        public int ExtraPrice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationDetails> ReservationDetails { get; set; }
    }
}
