namespace OniroHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rooms
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rooms()
        {
            ReservationDetails = new HashSet<ReservationDetails>();
        }

        [Key]
        public int RoomID { get; set; }

        public int RoomNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomName { get; set; }

        [Required]
        public string RoomImg { get; set; }

        [Required]
        public string RoomDesc { get; set; }

        public int RoomPrice { get; set; }

        public bool IsAvailable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationDetails> ReservationDetails { get; set; }
    }
}
