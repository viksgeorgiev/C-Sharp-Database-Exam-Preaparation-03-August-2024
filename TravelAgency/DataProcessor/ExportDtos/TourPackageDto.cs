using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("TourPackage")]
    public class TourPackageDto
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Description))]
        public string? Description { get; set; }

        [XmlElement(nameof(Price))]
        public decimal Price { get; set; }
    }
}