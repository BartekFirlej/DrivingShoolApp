namespace DrivingSchoolApp.DTOs
{
    public class DrivingLicenceRequestDTO
    {
        public DateTime ReceivedDate {  get; set; }
        public DateTime ExpirationDate { get; set; }
        public int LicenceCategoryId {  get; set; }
        public int CustomerId {  get; set; }
    }
}
