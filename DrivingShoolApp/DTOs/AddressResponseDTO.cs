namespace DrivingSchoolApp.DTOs
{
    public class AddressResponseDTO
    {
        public int Id { get; set; }

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public int Number { get; set; }
    }
}
