using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using INFM201.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace INFM201.Models
{
    public class Takeaway
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TakeawayID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get;set; } = DateTime.Now;

        public OrderStatusEnum OrderStatus { get; set; }
        public double TotalAmount { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}