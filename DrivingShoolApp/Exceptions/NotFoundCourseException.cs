using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCourseException : DataInconsistencyException
    {
        public NotFoundCourseException(int courseId) : base(String.Format("Not found course with id {0}.", courseId))
        {
            this.HResult = 404;
        }
    }
}
