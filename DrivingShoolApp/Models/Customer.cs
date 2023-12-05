namespace DrivingSchoolApp.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public virtual ICollection<DrivingLicence> DrivingLicences { get; set; } = new List<DrivingLicence>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    public virtual ICollection<DrivingLesson> DrivingLessons { get; set; } = new List<DrivingLesson>();
}
