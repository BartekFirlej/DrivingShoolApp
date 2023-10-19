namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundSubjectException : DataInconsistencyException
    {
        public NotFoundSubjectException() : base("Not found any subjects.")
        {
            this.HResult = 404;
        }

        public NotFoundSubjectException(int subjectId) : base(String.Format("Not found subject with id {0}.", subjectId))
        {
            this.HResult = 404;
        }
    }
}
