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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ConfirmationID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Confirm Email Address")]
        public string ConfirmEmailAddress { get; set; }

        [Display(Name = "Special Requests")]
        public string SpecialRequests { get; set; }
        public int? ReservationId { get; set; }
        public int? TakeawayId
        {get; set;}
    }
}