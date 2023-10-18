namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundRequiredLicenceCategoriesException : DataInconsistencyException
    {
        public NotFoundRequiredLicenceCategoriesException() : base(String.Format("Not found any requirements."))
        {
            this.HResult = 404;
        }
        public NotFoundRequiredLicenceCategoriesException(int licenceCategoryId) : base(String.Format("Not found any requirements for licence category with id {0}.",licenceCategoryId))
        {
            this.HResult = 404;
        }
        public NotFoundRequiredLicenceCategoriesException(int licenceCategoryId, int requiredLicenceCategoryId) 
              : base(String.Format("Licence category with id {0} is not required for category with id {1}.", requiredLicenceCategoryId, licenceCategoryId))
        {
            this.HResult = 404;
        }
    }
}
