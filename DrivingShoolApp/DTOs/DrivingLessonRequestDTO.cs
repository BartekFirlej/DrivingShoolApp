namespace DrivingSchoolApp.DTOs
{
    public class DrivingLessonRequestDTO
    {
        public DateTime LessonDate { get; set; }
        public int LecturerId { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public int CourseId {  get; set; }
    }
}
