using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            CustomerImportDto[]? customerImportDtos =
                XmlHelper.Deserialize<CustomerImportDto[]>(xmlString, "Customers");

            if (customerImportDtos != null && customerImportDtos.Length > 0)
            {
                ICollection<Customer> customersToAdd = new List<Customer>();

                foreach (CustomerImportDto customerImportDto in customerImportDtos)
                {
                    if (!IsValid(customerImportDto))
                    {
                        sb.AppendLine(string.Format(ErrorMessage));
                        continue;
                    }

                    Customer[] validCustomers = context
                        .Customers
                        .ToArray();

                    if (ExistsInDb(customerImportDto, validCustomers) 
                        || ExistsInDb(customerImportDto, customersToAdd))
                    {
                        sb.AppendLine(string.Format(DuplicationDataMessage));
                        continue;
                    }

                    Customer customer = new Customer()
                    {
                        FullName = customerImportDto.FullName,
                        Email = customerImportDto.Email,
                        PhoneNumber = customerImportDto.PhoneNumber
                    };

                    customersToAdd.Add(customer);
                    sb.AppendLine(string.Format(SuccessfullyImportedCustomer, customerImportDto.FullName));
                }

                context.AddRange(customersToAdd);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        private static bool ExistsInDb(CustomerImportDto customer, ICollection<Customer> dbCustomers)
        {
            return dbCustomers.Any(c =>
                c.FullName == customer.FullName
                || c.Email == customer.Email
                || c.PhoneNumber == customer.PhoneNumber);
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportBookingDto[]? bookingDtos = 
                JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString);

            if (bookingDtos != null && bookingDtos.Length > 0)
            {
                ICollection<Booking> bookingsToAdd = new List<Booking>();

                foreach (ImportBookingDto bookingDto in bookingDtos)
                {
                    if (!IsValid(bookingDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isValidDate = DateTime.TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBooking);

                    if (!isValidDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var customer = context
                        .Customers
                        .First(c => c.FullName == bookingDto.CustomerName);

                    var tourPackage = context
                        .TourPackages
                        .First(tp => tp.PackageName == bookingDto.TourPackageName);

                    Booking booking = new Booking()
                    {
                        BookingDate = dateOfBooking,
                        Customer = customer,
                        TourPackage = tourPackage
                    };

                    bookingsToAdd.Add(booking);

                    sb.AppendLine(string.Format(SuccessfullyImportedBooking,tourPackage.PackageName, dateOfBooking.ToString("yyyy-MM-dd")));
                }

                context.AddRange(bookingsToAdd);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
