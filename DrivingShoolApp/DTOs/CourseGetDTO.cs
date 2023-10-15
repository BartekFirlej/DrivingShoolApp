namespace DrivingSchoolApp.DTOs
{
    public class CourseGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BeginDate {  get; set; }
        public float Price { get; set; }
        public int Limit {  get; set; }
        public CourseTypeGetDTO CourseType {  get; set; }
    }
}
