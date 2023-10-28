namespace DrivingSchoolApp.Exceptions
{
    public class SubjectAlreadyConductedLectureException : DataInconsistencyException
    {
        public SubjectAlreadyConductedLectureException(int courseId, int subjectId) : base(String.Format("There was lecture about subject with id {0} already conducted during course with id {1}.", subjectId, courseId))
        {
            this.HResult = 409;
        }
    }
}
