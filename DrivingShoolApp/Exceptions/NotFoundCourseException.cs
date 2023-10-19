namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCourseException : DataInconsistencyException
    {
        public NotFoundCourseException() : base(String.Format("Not found any courses."))
        {
            this.HResult = 404;
        }
        public NotFoundCourseException(int courseId) : base(String.Format("Not found course with id {0}.", courseId))
        {
            this.HResult = 404;
        }
    }
}
