using System;
using System.Collections.Generic;
using System.Text;

namespace ErtanSahin.WindowsApp
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public DateTime RezervationStart { get; set; }
        public DateTime RezervationEnd { get; set; }
        public List<Member> Members { get; set; } = new List<Member>();
        public class Member
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string State { get; set; }
        }
    }
}
