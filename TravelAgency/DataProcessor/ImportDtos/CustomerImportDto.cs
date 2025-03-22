﻿namespace TravelAgency.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Serialization;


    [XmlType("Customer")]
    public class CustomerImportDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        [XmlElement(nameof(FullName))]
        public string FullName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        [XmlElement(nameof(Email))]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"\+\d{12}")]
        [XmlAttribute("phoneNumber")]
        public string PhoneNumber { get; set; } = null!;
    }
}
