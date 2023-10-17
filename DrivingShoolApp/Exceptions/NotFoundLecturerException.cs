namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundLecturerException : DataInconsistencyException
    {
        public NotFoundLecturerException(int lecturerId) : base(String.Format("Not found lecturer with id {0}.", lecturerId))
        {
            this.HResult = 404;
        }
    }
}
