using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundRegistrationsException : DataInconsistencyException
    {
        public NotFoundRegistrationsException() : base(String.Format("Not found any registrations."))
        {
            this.HResult = 404;
        }
    }
}
