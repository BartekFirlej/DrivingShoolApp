namespace DrivingSchoolApp.DTOs
{
    public class LectureResponseDTO
    {
        public int Id { get; set; }

        public DateTime LectureDate { get; set; }

        public int ClassroomId { get; set; }

        public int LecturerId { get; set; }

        public int CourseSubjectsSubjectId { get; set; }

        public int CourseSubjectsCourseId { get; set; }

    }
}
