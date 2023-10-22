using System.ComponentModel.DataAnnotations;

namespace VegHouse.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }



        [Display(Name = "Oluşturma Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }




        public List<Product> Products { get; set; } = new List<Product>();

    }
}
    