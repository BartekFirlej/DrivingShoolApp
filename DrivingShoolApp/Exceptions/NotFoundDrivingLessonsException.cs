namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundDrivingLessonsException : DataInconsistencyException
    {
        public NotFoundDrivingLessonsException() : base(String.Format("Not found any driving lessons."))
        {
            this.HResult = 404;
        }
    }
}
