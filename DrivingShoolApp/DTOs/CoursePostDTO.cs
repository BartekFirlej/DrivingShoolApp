namespace DrivingSchoolApp.DTOs
{
    public class CoursePostDTO
    {
        public string Name { get; set; }
        public DateTime BeginDate { get; set; }
        public int Price { get; set; }
        public int Limit { get; set; }
        public int CourseTypeId { get; set; }
    }
}
