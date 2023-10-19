namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundDrivingLicenceException : DataInconsistencyException
    {
        public NotFoundDrivingLicenceException() : base(String.Format("Not found any driving licences."))
        {
            this.HResult = 404;
        }

        public NotFoundDrivingLicenceException(int drivingLicenceId) : base(String.Format("Not found driving licence with id {0}.", drivingLicenceId))
        {
            this.HResult = 404;
        }
    }
}
