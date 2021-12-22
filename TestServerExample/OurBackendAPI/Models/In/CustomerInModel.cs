using System.ComponentModel.DataAnnotations;

namespace OurBackendAPI.Models.In
{
    public class CustomerInModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string CompanyName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public string Phone { get; set; }
    }
}
