namespace OniroHotel.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class ReservationDetails
    {
        [Key]
        public int ResDetID { get; set; }

        public int ResID { get; set; }

        public int RoomID { get; set; }

        public int? ExtraID { get; set; }

        public virtual Extras Extras { get; set; }

        public virtual Reservations Reservations { get; set; }

        public virtual Rooms Rooms { get; set; }
    }
}
