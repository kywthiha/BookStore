using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ModifedDate { get; set; }
        public ICollection<Book> Books { get; set; }
        

    }
}
