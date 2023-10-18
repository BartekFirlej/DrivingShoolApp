namespace DrivingSchoolApp.Models;

public partial class RequiredLicenceCategory
{
    public int LicenceCategoryId { get; set; }

    public int RequiredLicenceCategoryId { get; set; }

    public int RequiredYears { get; set; }

    public virtual LicenceCategory LicenceCategory { get; set; } = null!;

    public virtual LicenceCategory RequiredLicenceCategories { get; set; } = null!;
}
