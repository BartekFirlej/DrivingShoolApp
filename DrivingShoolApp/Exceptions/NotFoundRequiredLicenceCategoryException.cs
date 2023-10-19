namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundRequiredLicenceCategoryException : DataInconsistencyException
    {
        public NotFoundRequiredLicenceCategoryException() : base(String.Format("Not found any requirements."))
        {
            this.HResult = 404;
        }
        public NotFoundRequiredLicenceCategoryException(int licenceCategoryId) : base(String.Format("Not found any requirements for licence category with id {0}.",licenceCategoryId))
        {
            this.HResult = 404;
        }
        public NotFoundRequiredLicenceCategoryException(int licenceCategoryId, int requiredLicenceCategoryId) 
              : base(String.Format("Licence category with id {0} is not required for category with id {1}.", requiredLicenceCategoryId, licenceCategoryId))
        {
            this.HResult = 404;
        }
    }
}
