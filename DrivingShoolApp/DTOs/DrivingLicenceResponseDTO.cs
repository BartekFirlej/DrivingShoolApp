namespace DrivingSchoolApp.DTOs
{
    public class DrivingLicenceResponseDTO
    {
        public int Id { get; set; }

        public DateTime ReceivedDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int CustomerId { get; set; }

        public int LicenceCategoryId { get; set; }
    }
}
