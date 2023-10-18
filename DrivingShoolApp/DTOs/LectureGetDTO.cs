namespace DrivingSchoolApp.DTOs
{
    public class LectureGetDTO
    {
        public int Id {  get; set; }
        public DateTime LectureDate {  get; set; }
        public int LecturerId { get; set; }
        public string LecturerName { get; set; }
        public int CourseId {  get; set; }
        public string CourseName {  get; set; }
        public int SubjectId {  get; set; }
        public string SubjectName { get; set; }
        public int ClassroomNumber {  get; set; }
        public string City {  get; set; }
        public string Street { get; set; }
        public int Number {  get; set; }
    }
}
