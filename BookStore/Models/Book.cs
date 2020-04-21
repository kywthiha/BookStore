using BookStore.Pages.Admin.Util;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        
        public Author Author { get; set; }
        [Required]
        public int AuthorID { get; set; }
        public string Summary { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        [Display(Name ="Unit Price")]
        public double Price { get; set; }
        public Category Category { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public string Cover { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ModifedDate { get; set; }
        [NotMapped]
        public OrderItem OrderItem { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = "Cover")]
        [MaxFileSize(5 *1024* 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile FormFile { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
