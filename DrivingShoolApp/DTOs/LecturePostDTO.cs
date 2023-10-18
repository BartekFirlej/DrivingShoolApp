namespace DrivingSchoolApp.DTOs
{
    public class LecturePostDTO
    {
        public DateTime LectureDate {  get; set; }
        public int LecturerId { get; set; }
        public int SubjectId {  get; set; }
        public int ClassroomId {  get; set; }
        public int CourseId { get; set; }
    }
}
