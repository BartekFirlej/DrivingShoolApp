namespace DrivingSchoolApp.Exceptions
{
    public class RequirementAlreadyExistsException : DataInconsistencyException
    {
        public RequirementAlreadyExistsException(int requiredLicenceCategoryId, int licenceCategoryId) : base(String.Format("There is category with id {0} already required for category with id {1}.", requiredLicenceCategoryId, licenceCategoryId))
        {
            this.HResult = 409;
        }
    }
}
