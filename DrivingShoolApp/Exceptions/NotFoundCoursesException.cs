using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCoursesException : DataInconsistencyException
    {
        public NotFoundCoursesException() : base(String.Format("Not found any courses."))
        {
            this.HResult = 404;
        }
    }
}
