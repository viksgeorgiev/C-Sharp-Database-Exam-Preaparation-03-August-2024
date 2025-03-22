using System.Xml.Serialization;
using TravelAgency.Data.Models;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]
    public class GuidesExportDto
    {
        [XmlElement(nameof(FullName))]
        public string FullName { get; set; } = null!;

        [XmlArray("TourPackages")]
        public TourPackageDto[] TourPackages { get; set; } 

    }
}
