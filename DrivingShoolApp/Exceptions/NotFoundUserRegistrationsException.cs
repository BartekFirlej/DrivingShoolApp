using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundUserRegistrationsException : DataInconsistencyException
    {
        public NotFoundUserRegistrationsException(int userId) : base(String.Format("Not found any registrations for user with id {0}.", userId))
        {
            this.HResult = 404;
        }
    }
}
