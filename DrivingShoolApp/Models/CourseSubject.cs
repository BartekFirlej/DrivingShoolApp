namespace DrivingSchoolApp.Models;

public partial class CourseSubject
{
    public int SubjectId { get; set; }

    public int CourseId { get; set; }

    public int SequenceNumber { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    public virtual Subject Subject { get; set; } = null!;
}
