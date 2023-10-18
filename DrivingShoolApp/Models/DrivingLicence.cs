namespace DrivingSchoolApp.Models;

public partial class DrivingLicence
{
    public int Id { get; set; }

    public DateTime ReceivedDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int CustomerId { get; set; }

    public int LicenceCategoryId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual LicenceCategory LicenceCategory { get; set; } = null!;

}
