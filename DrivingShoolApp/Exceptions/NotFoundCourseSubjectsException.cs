namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCourseSubjectsException : DataInconsistencyException
    {
        public NotFoundCourseSubjectsException() : base(String.Format("Not found any courses with subjects."))
        {
            this.HResult = 404;
        }
    }
}
