using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace INFM201.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }

        [Required]
        [Display(Name = "Guest Fullnames")]
        public string CFullnames { get; set; }

        [Required]
        [Display(Name = "Guest Email")]
        [DataType(DataType.EmailAddress)]
        public string CEmail { get; set; }
       
        [Required(ErrorMessage = "Please enter a reservation date.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter a reservation time.")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Please enter the number of guests.")]
        [Range(1, int.MaxValue, ErrorMessage = "Party size must be at least 1.")]
        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }

        [Required(ErrorMessage = "Please specify seating preference.")]
        [Display(Name = "Seating Preference")]
        public string SeatingPreference { get; set; }

        [Display(Name = "Special Requests")]
        public string SpecialRequests { get; set; }

        public bool IsConfirmed { get; set; }

        public int NumberOfBookingsInside { get; set; }
        public int NumberOfBookingsOutside { get; set; }

        [Display(Name = "Seating Type (Inside)")]
        public string SeatingTypeInside { get; set; } 
        public bool IsDeleted { get; set; } // Soft delete

        public bool IsCompleted { get; set; } = false; // Default value to false

        public virtual Confirmation Confirmation { get; set; }

        public int TableID { get; set; } 

        [ForeignKey("TableID")]
        public virtual Table Table { get; set; }


    }
}
 























   