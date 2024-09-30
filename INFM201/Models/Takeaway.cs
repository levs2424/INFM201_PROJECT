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
        public int Quantity { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }



        //[Required]
        //[Display(Name = "Please select a menu item")]
        public string OrderItem { get; set; }
        public double GetPrice()
        {
            double price=0;
            foreach (var item in OrderItems)
            {
                switch (item.ItemName)
                {
                    case "Margherita Pizza":
                        price += 60;
                        break;

                    case "Chicken Mayo Pizza":
                        price += 80;
                        break;

                    case "BBQ Chicken Burger":
                        price += 70;
                        break;

                    case "Cheeseburger":
                        price += 75;
                        break;

                    case "Chicken Wrap":
                        price += 65;
                        break;

                    case "Veggie Wrap ":
                        price += 55;
                        break;

                    case "Fish & Chips":
                        price += 85;
                        break;

                    case "Beef Lasagna":
                        price += 95;
                        break;

                    case "Grilled Chicken Salad":
                        price += 70;
                        break;

                    case "Greek Salad":
                        price += 50;
                        break;

                    default:
                        break;
                }
            }

            double final_price = Quantity * price;
            return final_price;

        }
    }
}