using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Order
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.MultilineText)]
        public bool Status { get; set; }
        public string Address { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ModifedDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
