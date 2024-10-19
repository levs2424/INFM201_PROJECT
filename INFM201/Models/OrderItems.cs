using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace INFM201.Models
{
    public class OrderItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemID { get; set; }

        public string ItemName { get; set; }

        public int Quantity { get; set; }


        public double ItemPrice { get; set; }

        public virtual Takeaway Takeaway { get; set; }


    }
}