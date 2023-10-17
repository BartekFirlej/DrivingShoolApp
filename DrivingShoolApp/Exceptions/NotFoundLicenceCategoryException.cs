namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundLicenceCategoryException : DataInconsistencyException
    {
        public NotFoundLicenceCategoryException(int licenceCategoryId) : base(String.Format("Not found licence category with id {0}.", licenceCategoryId))
        {
            this.HResult = 404;
        }
    }
}
