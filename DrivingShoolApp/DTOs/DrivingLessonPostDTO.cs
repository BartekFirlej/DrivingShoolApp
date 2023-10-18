namespace DrivingSchoolApp.DTOs
{
    public class DrivingLessonPostDTO
    {
        public DateTime LessonDate { get; set; }
        public int LecturerId { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
    }
}
