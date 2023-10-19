namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCourseSubjectException : DataInconsistencyException
    {
        public NotFoundCourseSubjectException() : base(String.Format("Not found any courses with subjects."))
        {
            this.HResult = 404;
        }
        public NotFoundCourseSubjectException(int courseId, int subjectId) : base(String.Format("There is not subject with id {0} at course with id {1}.",subjectId, courseId))
        {
            this.HResult = 404;
        }
    }
}
