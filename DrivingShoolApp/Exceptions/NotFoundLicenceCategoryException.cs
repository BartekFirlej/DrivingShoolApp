namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundLicenceCategoryException : DataInconsistencyException
    {
        public NotFoundLicenceCategoryException() : base(String.Format("Not found any licence category."))
        {
            this.HResult = 404;
        }

        public NotFoundLicenceCategoryException(int licenceCategoryId) : base(String.Format("Not found licence category with id {0}.", licenceCategoryId))
        {
            this.HResult = 404;
        }
    }
}
