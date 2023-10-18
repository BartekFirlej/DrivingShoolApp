namespace DrivingSchoolApp.DTOs
{
    public class CustomerLectureGetDTO
    {
        public int CustomerId {  get; set; }
        public string CustomerName { get; set; }
        public int LectureId { get; set; }
        public DateTime LectureDate {  get; set; }
    }
}
