namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundLectureException : DataInconsistencyException
    {
        public NotFoundLectureException(int lecturerId) : base(String.Format("Not found lecture with id {0}.", lecturerId))
        {
            this.HResult = 404;
        }
    }
}
