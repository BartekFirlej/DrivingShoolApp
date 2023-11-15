namespace DrivingSchoolApp.Models;

public partial class DrivingLesson
{
    public int Id { get; set; }

    public DateTime LessonDate { get; set; }

    public int AddressId { get; set; }

    public int LecturerId { get; set; }

    public int CustomerId { get; set; }
    public int CourseId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Lecturer Lecturer { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}
