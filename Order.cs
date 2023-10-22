using System.ComponentModel.DataAnnotations;
using VegHouse.Areas.Identity.Data;

namespace VegHouse.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;


        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Adres { get; set; }
        public string? Il { get; set; }
        public string? Ilce { get; set; }
        public string? Eposta { get; set; }
        public string? Tel { get; set; }



        public string? KargoAdi { get; set; }
        public string? KargoTakipNo { get; set; }

        public string? OrderStatus { get; set; }
        public string? OdemeYontemi { get; set; }

        public decimal OrderPrice { get; set; }


        public List<OrderDetails>? OrderDetails { get; set; }


        public int UserId { get; set; }
        public Customers? Customers { get; set; }

    }

}
