using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCourseTypeException : DataInconsistencyException
    {
        public NotFoundCourseTypeException(int courseTypeId) : base(String.Format("Not found course type with id {0}.", courseTypeId))
        {
            this.HResult = 404;
        }
    }
}
