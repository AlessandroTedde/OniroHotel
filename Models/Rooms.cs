namespace OniroHotel.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Rooms
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rooms()
        {
            ReservationDetails = new HashSet<ReservationDetails>();
        }

        [Key]
        public int RoomID { get; set; }
        [Display(Name = "Numero Camera")]
        public int RoomNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nome Camera")]
        public string RoomName { get; set; }

        [Required]
        [Display(Name = "Immagine")]
        public string RoomImg { get; set; }

        [Required]
        [Display(Name = "Descrizione")]
        public string RoomDesc { get; set; }
        [Display(Name = "Prezzo")]
        public int RoomPrice { get; set; }
        [Display(Name = "Disponibile")]
        public bool IsAvailable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationDetails> ReservationDetails { get; set; }
    }
}
