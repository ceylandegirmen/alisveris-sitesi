using System.ComponentModel.DataAnnotations;

namespace VegHouse.Models
{
    public class Customers
    {
        [Key]
        public int UserId { get; set; }



        [Display(Name = "Kullanıcı Adı")]

        public string? Username { get; set; }


        [Required]
        [Display(Name = "Adınız ")]
        public string? Name { get; set; }



        [Required]
        [Display(Name = "Soyadınız ")]
        public string? SurName { get; set; }



        

        [Required]
        [Display(Name = "Eposta")]
        [EmailAddress(ErrorMessage = "E-posta giriniz.")]
        public string? Email { get; set; }



        [Required]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }




        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifrenizi giriniz.")]
        public string? RePassword { get; set; }






        [Display(Name = "Telefon")]
        public string? Gsm { get; set; }




        [Display(Name = "Adresiniz")]
        public string? Address { get; set; }





        [Display(Name = "Tc")]
        public long? Tc { get; set; }






        //public  Cart? Cart { get; set; } = null!; //kayıt yapmıyor



        //public int CartId { get; set; }
        //public ICollection<Cart>? Carts { get; set; } = null!; //böyleyken kayıt yapabiliyor


    }
}
