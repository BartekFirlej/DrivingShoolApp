namespace DrivingSchoolApp.Models;

public partial class RequiredDrivingLicence
{
    public int DrivingLicenceId { get; set; }

    public int RequiredDrivingLicenceId { get; set; }

    public int RequiredYears { get; set; }

    public virtual DrivingLicence DrivingLicence { get; set; } = null!;

    public virtual DrivingLicence RequiredDrivingLicenceNavigation { get; set; } = null!;
}
