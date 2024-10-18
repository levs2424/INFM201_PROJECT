using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INFM201.Models
{
    public class Confirmation
    {
        [Key, ForeignKey("Reservation")]
        public int ReservationID { get; set; }

        public string Name { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Special Requests")]
        public string SpecialRequests { get; set; }
    }
}