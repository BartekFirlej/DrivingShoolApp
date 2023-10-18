namespace DrivingSchoolApp.DTOs
{
    public class RequiredLicenceCategoryGetDTO
    {
        public int LicenceCategoryId {  get; set; }
        public string LicenceCategoryName { get; set; }
        public int RequiredLicenceCategoryId { get; set; }
        public string RequiredLicenceCategoryName { get; set; }
        public int RequiredYears {  get; set; }
    }
}
