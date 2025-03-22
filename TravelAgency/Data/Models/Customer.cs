namespace TravelAgency.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        public string FullName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [Required]
        [Column(TypeName = @"VARCHAR(13)")]
        [RegularExpression(@"\+\d{12}")]
        public string PhoneNumber { get; set; } = null!;

        public virtual HashSet<Booking> Bookings { get; set; }
            = new HashSet<Booking>();
    }

  
}
