namespace DrivingSchoolApp.DTOs
{
    public class DrivingLicencePostDTO
    {
        public DateTime ReceivedDate {  get; set; }
        public DateTime ExpirationDate { get; set; }
        public int LicenceCategoryId {  get; set; }
        public int UserId {  get; set; }
    }
}
