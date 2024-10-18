using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace INFM201.Models
{
    public class Table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableID { get; set; }

        public string TableNumber { get; set; }

        public string SeatingType { get; set; } 

        public int MaxGuests { get; set; } 

        public bool IsAvailable { get; set; } 

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}