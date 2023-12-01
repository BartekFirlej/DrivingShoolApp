namespace DrivingSchoolApp.DTOs
{
    public class CourseTypeResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int MinimumAge { get; set; }

        public int LectureHours { get; set; }

        public int DrivingHours { get; set; }

        public int LicenceCategoryId { get; set; }
    }
}
