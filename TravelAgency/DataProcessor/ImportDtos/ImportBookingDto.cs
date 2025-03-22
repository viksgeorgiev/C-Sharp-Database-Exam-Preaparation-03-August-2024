namespace TravelAgency.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class ImportBookingDto
    {
        [Required]
        public string BookingDate { get; set; } = null!;

        [Required]
        public string CustomerName { get; set; } = null!;

        [Required]
        public string TourPackageName { get; set; } = null!;
    }
}
