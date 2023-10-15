namespace DrivingSchoolApp.Models;

public partial class Lecturer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public virtual ICollection<DrivingLesson> DrivingLessons { get; set; } = new List<DrivingLesson>();

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
