using System.ComponentModel.DataAnnotations;
using TravelAgency.Data.Models.Enums;

namespace TravelAgency.Data.Models
{
    public class Guide
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        public string FullName { get; set; } = null!;

        [Required]
        public Language Language { get; set; }

        public virtual HashSet<TourPackageGuide> TourPackagesGuides { get; set; }
            = new HashSet<TourPackageGuide>();

    }
}
