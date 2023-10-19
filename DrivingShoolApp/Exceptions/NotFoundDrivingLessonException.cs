namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundDrivingLessonException : DataInconsistencyException
    {
        public NotFoundDrivingLessonException() : base(String.Format("Not found any driving lessons."))
        {
            this.HResult = 404;
        }

        public NotFoundDrivingLessonException(int drivingLessonId) : base(String.Format("Not found driving lesson with id {0}.", drivingLessonId))
        {
            this.HResult = 404;
        }
    }
}
