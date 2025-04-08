using System.Collections.Generic;

namespace OniroHotel.Models
{
    public class CartViewModel
    {
        public Rooms Room { get; set; }
        public List<Extras> Extras { get; set; }
        public List<Extras> SelectedExtras { get; set; }
        public Reservations Reservation { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public long? UserPhone { get; set; }
    }
}