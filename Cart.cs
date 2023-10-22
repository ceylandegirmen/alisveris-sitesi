using System.ComponentModel.DataAnnotations;
using VegHouse.Areas.Identity.Data;

namespace VegHouse.Models
{
    public class Cart
    {

        [Key]
        public int CartId { get; set; }
    

        public bool IsDeleted { get; set; } = false;
       

        public ICollection<CartDetails>? CartDetails { get; set; }


        public ICollection<Customers>? Customers2 { get; set; } // çalışmıyor




        public int UserId { get; set; }
        public Customers? Customers { get; set; }


    }
}
