using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class OrderItem
    {
        public int ID { get; set; }
        [Required]
        public int BookID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public Book Book { get; set; }
        [Required]
        public Order Order { get; set; }
        [Required]
        public int Qty { get; set; }
        [DataType(DataType.Currency)]
        [NotMapped]
        [Display(Name ="Price")]
        public double Total { get; set; }
 
    }
}
