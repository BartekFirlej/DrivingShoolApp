namespace DrivingSchoolApp.Models;

public partial class CourseType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int MinimumAge { get; set; }

    public int LectureHours { get; set; }

    public int DrivingHours { get; set; }

    public int LicenceCategoryId { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual LicenceCategory LicenceCategory { get; set; } = null!;
}
