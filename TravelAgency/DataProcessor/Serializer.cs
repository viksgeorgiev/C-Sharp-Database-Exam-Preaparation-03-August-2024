using Newtonsoft.Json;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var guides = context
                .Guides
                .Where(g => g.Language == Language.Spanish)
                .OrderByDescending(g => g.TourPackagesGuides.Count)
                .ThenBy(g => g.FullName)
                .Select(g => new GuidesExportDto()
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                        .Select(tp => new TourPackageDto()
                        {
                            Name = tp.TourPackage.PackageName,
                            Description = tp.TourPackage.Description,
                            Price = tp.TourPackage.Price
                        })
                        .OrderByDescending(tpdto => tpdto.Price)
                        .ThenBy(tpdto => tpdto.Name)
                        .ToArray()
                })
                .ToArray();

            string output = XmlHelper.Serialize(guides, "Guides");

            return output;
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customers = context
                .Customers
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                
                .Select(c => new
                {
                    c.FullName,
                    c.PhoneNumber,
                    Bookings = c.Bookings
                        .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                        .OrderBy(b => b.BookingDate)
                        .Select(b => new
                        {
                            TourPackageName = b.TourPackage.PackageName,
                            Date = b.BookingDate.ToString("yyyy-MM-dd")
                        })
                        .ToArray()
                })
                .OrderByDescending(c => c.Bookings.Length)
                .ThenBy(c => c.FullName)
                .ToArray();


            string result = JsonConvert.SerializeObject(customers,Formatting.Indented);

            return result;
        }
    }
}
