namespace DrivingSchoolApp.DTOs
{
    public class DrivingLicenceGetDTO
    {
        public int Id {  get; set; }
        public int UserId {  get; set; }
        public string UserName { get; set; }
        public string UserSecondName {  get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime ExpirationDate {  get; set; }
        public int LicenceCategoryId {  get; set; }
        public string LicenceCategoryName { get; set;}
    }
}
