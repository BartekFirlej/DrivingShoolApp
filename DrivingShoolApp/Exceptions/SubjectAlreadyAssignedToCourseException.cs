namespace DrivingSchoolApp.Exceptions
{
    public class SubjectAlreadyAssignedToCourseException : DataInconsistencyException
    {
        public SubjectAlreadyAssignedToCourseException(int courseId, int subjectId) : base(String.Format("There is subject with id {0} already assgined to course with id {1}.", subjectId, courseId))
        {
            this.HResult = 409;
        }
    }
}
