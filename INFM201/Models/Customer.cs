using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INFM201.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "Guest Fullnames")]
        public string Fullnames { get; set; }

        [Required]
        [Display(Name = "Guest Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}