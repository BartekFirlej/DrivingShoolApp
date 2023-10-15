using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundDrivingLicenceException : DataInconsistencyException
    {
        public NotFoundDrivingLicenceException(int drivingLicenceId) : base(String.Format("Not found driving licence with id {0}.", drivingLicenceId))
        {
            this.HResult = 404;
        }
    }
}
