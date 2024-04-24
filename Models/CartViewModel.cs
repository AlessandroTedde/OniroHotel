using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OniroHotel.Models
{
    public class CartViewModel
    {
        public Rooms Room { get; set; }
        public List<Extras> Extras { get; set; }
        public Reservations Reservation { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public long? UserPhone { get; set; }
    }
}