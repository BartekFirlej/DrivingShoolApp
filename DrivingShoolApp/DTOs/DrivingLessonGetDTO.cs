namespace DrivingSchoolApp.DTOs
{
    public class DrivingLessonGetDTO
    {
        public int Id {  get; set; }
        public DateTime LessonDate { get; set; }
        public int LecturerId {  get; set; }
        public string LecturerName {  get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int AddressId { get; set; }
    }
}
