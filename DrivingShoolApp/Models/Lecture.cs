namespace DrivingSchoolApp.Models;

public partial class Lecture
{
    public int Id { get; set; }

    public DateTime LectureDate { get; set; }

    public int ClassroomId { get; set; }

    public int LecturerId { get; set; }

    public int CourseSubjectsSubjectId { get; set; }

    public int CourseSubjectsCourseId { get; set; }

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual CourseSubject CourseSubjects { get; set; } = null!;

    public virtual Lecturer Lecturer { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
