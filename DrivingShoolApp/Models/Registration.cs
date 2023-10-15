namespace DrivingSchoolApp.Models;

public partial class Registration
{
    public DateTime RegistrationDate { get; set; }

    public int CustomerId { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
