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

        public string SeatingType { get; set; } // "Inside - Couch/Lounge", "Inside - Table", or "Outside"

        public int MaxGuests { get; set; } // Max guests per table

        public bool IsAvailable { get; set; } // Track availability
    }
}