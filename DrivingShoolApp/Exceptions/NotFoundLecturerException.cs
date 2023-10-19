namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundLecturerException : DataInconsistencyException
    {
        public NotFoundLecturerException() : base(String.Format("Not found any lecturer."))
        {
            this.HResult = 404;
        }

        public NotFoundLecturerException(int lecturerId) : base(String.Format("Not found lecturer with id {0}.", lecturerId))
        {
            this.HResult = 404;
        }
    }
}
