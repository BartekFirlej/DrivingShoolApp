using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundRegistrationException : DataInconsistencyException
    {
        public NotFoundRegistrationException(int customerId, int courseId) : base(String.Format("Not found registration from customer with id {0} for course with id {1}.", customerId, courseId))
        {
            this.HResult = 404;
        }
    }
}
