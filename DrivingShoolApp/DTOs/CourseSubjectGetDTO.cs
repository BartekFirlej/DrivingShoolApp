namespace DrivingSchoolApp.DTOs
{
    public class CourseSubjectGetDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int SubjectId {  get; set; }
        public string SubjectName {  get; set; }
        public int Duration { get; set; }
        public int SequenceNumber { get; set; }
    }
}
