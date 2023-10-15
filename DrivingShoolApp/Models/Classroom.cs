namespace DrivingSchoolApp.Models;

public partial class Classroom
{
    public int Id { get; set; }

    public int Number { get; set; }

    public int Size { get; set; }

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
