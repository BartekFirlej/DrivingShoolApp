namespace DrivingSchoolApp.DTOs
{
    public class CourseTypeRequestDTO
    {
        public string Name {  get; set; }
        public int MinimumAge { get; set; }
        public int LectureHours { get; set; }
        public int DrivingHours { get; set; }
        public int LicenceCategoryId { get; set; } 
    }
}
