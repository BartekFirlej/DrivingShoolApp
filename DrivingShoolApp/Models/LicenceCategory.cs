namespace DrivingSchoolApp.Models;

public partial class LicenceCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CourseType> CourseTypes { get; set; } = new List<CourseType>();

    public virtual ICollection<DrivingLicence> DrivingLicences { get; set; } = new List<DrivingLicence>();

    public virtual ICollection<RequiredLicenceCategory> RequiredDrivingLicenceDrivingLicences { get; set; } = new List<RequiredLicenceCategory>();

    public virtual ICollection<RequiredLicenceCategory> RequiredDrivingLicenceRequiredDrivingLicenceNavigations { get; set; } = new List<RequiredLicenceCategory>();
}
