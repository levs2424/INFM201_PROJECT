using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INFM201.Models
{
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }

        [Required]
        [DisplayName("Employee ID")]
        public int EmployeeID { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string StaffEmail { get; set; }
        
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true; // Can deactivate users

        // Relationships
        public virtual ICollection<Takeaway> Takeaway { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }



    }











}
