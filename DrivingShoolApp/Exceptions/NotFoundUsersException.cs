using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundUsersException : DataInconsistencyException
    {
        public NotFoundUsersException() : base(String.Format("Not found any users."))
        {
            this.HResult = 404;
        }
    }
}
