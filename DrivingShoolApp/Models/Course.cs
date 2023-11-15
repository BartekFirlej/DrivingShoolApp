namespace DrivingSchoolApp.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime BeginDate { get; set; }

    public float Price { get; set; }

    public int Limit { get; set; }

    public int CourseTypeId { get; set; }

    public virtual ICollection<CourseSubject> CourseSubjects { get; set; } = new List<CourseSubject>();

    public virtual CourseType CourseType { get; set; } = null!;

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<DrivingLesson> DrivingLessons { get; set; } = new List<DrivingLesson>();
}
