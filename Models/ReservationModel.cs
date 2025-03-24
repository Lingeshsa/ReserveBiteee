using System;
using System.ComponentModel.DataAnnotations;

namespace ReserveBiteee.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public int Guests { get; set; }
        public string? Requests { get; set; }
        public string Categorie { get; set; }

        public TableModelAC TableModelAc;

        public TableModelNonAc TableModelNonAc;

        public string Status { get; set; } = "Pending";
    }
}
