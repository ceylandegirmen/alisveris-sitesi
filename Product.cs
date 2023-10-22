using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VegHouse.Models
{
    public class Product
    {
        public int ProductId { get; set; }


        [Display(Name = "Ürün Adı")]
        public string? ProductName { get; set; }




        [Display(Name = "Ürün Açıklaması")]
        public string? ProductDescription { get; set; }




        [Display(Name = "Ürün fiyatı")]
        public decimal ProductPrice { get; set; }





        [Display(Name = "Oluşturma Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }




        [Display(Name = "Fotoğraf")]
        public string? ImagePath { get; set;  } 




        [NotMapped]
        public IFormFile? ImageFile { set; get; }

   

        //public List<Category> categories { get; set; } = new List<Category>();
        // [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
      
        public Category? Category { get; set; } 





        //public ICollection<Order>? Orders { get; set; }

        //public ICollection<OrderStatus>? OrderStatuses { get; set; }

        public ICollection<CartDetails>? CartDetails { get; set; }

    }
}
