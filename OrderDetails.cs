using System.ComponentModel.DataAnnotations;

namespace VegHouse.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrDetailsId { get; set; }

        public int Quantity { get; set; }

        public decimal ProductPrice { get; set; }



        public int OrderId { get; set; }
        public Order? Order { get; set; } = null!;


        public int ProductId { get; set; }
        public Product? Product { get; set; } = null!;
    }
}
