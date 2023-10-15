using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundDrivingLicencesException : DataInconsistencyException
    {
        public NotFoundDrivingLicencesException() : base(String.Format("Not found any driving licences."))
        {
            this.HResult = 404;
        }
    }
}
