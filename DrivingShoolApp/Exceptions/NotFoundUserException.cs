using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundUserException : DataInconsistencyException
    {
        public NotFoundUserException(int userId) : base(String.Format("Not found user with id {0}.", userId))
        {
            this.HResult = 404;
        }
    }
}
