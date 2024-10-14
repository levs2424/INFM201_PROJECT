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


        [Required]
        [Display(Name = "Please enter your Full names")]
        public string Fullnames{ get; set; }

        [Required]
        [Display(Name = "Guest Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        public DateTime OrderDate { get;set; } = DateTime.Now;

        public OrderStatusEnum OrderStatus { get; set; }
        public double TotalAmount { get; set; }

        public bool IsDelete { get; set; } = false;
       

        public virtual ICollection<OrderItems> OrderItems { get; set; }

        public double GetPrice()
        {
         
            double totalprice = 0;
            foreach (var item in OrderItems)
            {
                double price = 0;
                switch (item.ItemName)
                {
                    case "Margherita Pizza":
                        price = 60;
                        item.ItemPrice = price;
                        break;

                    case "Chicken Mayo Pizza":
                        price = 80;
                        item.ItemPrice = price;
                        break;

                    case "BBQ Chicken Burger":
                        price = 70;
                        item.ItemPrice = price;
                        break;

                    case "Cheeseburger":
                        price = 75;
                        item.ItemPrice = price;
                        break;

                    case "Chicken Wrap":
                        price = 65;
                        item.ItemPrice = price;
                        break;

                    case "Veggie Wrap ":
                        price = 55;
                        item.ItemPrice = price;
                        break;

                    case "Fish & Chips":
                        price = 85;
                        item.ItemPrice = price;
                        break;

                    case "Beef Lasagna":
                        price = 95;
                        item.ItemPrice = price;
                        break;

                    case "Grilled Chicken Salad":
                        price = 70;
                        item.ItemPrice = price;
                        break;

                    case "Greek Salad":
                        price = 50;
                        item.ItemPrice = price;
                        break;

                    default:
                        break;
                }

                totalprice = totalprice + (price * item.Quantity);
            }

            
            return totalprice;

        }
    }
}