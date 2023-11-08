namespace DrivingSchoolApp.DTOs
{
    public class DrivingLicenceGetDTO
    {
        public int Id {  get; set; }
        public int CustomerId {  get; set; }
        public string CustomerName { get; set; }
        public string CutomserSecondName {  get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? ExpirationDate {  get; set; }
        public int LicenceCategoryId {  get; set; }
        public string LicenceCategoryName { get; set;}
    }
}
