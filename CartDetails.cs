using System.ComponentModel.DataAnnotations;

namespace VegHouse.Models
{
    public class CartDetails
    {
       [Key]
        public int CartDetailsId { get; set; }

        public int Quantity { get; set; }

        public decimal ProductPrice { get; set; }


        public DateTime CreateDate { get; set; } = DateTime.UtcNow;


        //ilişkiler

        public int CartId { get; set; }
        public Cart? Cart { get; set; } = null!; 



        public int ProductId { get; set; }
        public Product? Product { get; set; } = null!; 

        
    }
}
