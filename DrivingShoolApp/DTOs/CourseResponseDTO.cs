namespace DrivingSchoolApp.DTOs
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime BeginDate { get; set; }

        public float Price { get; set; }

        public int Limit { get; set; }

        public int CourseTypeId { get; set; }
    }
}
