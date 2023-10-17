namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundLecturersException : DataInconsistencyException
    {
        public NotFoundLecturersException() : base(String.Format("Not found any lecturer."))
        {
            this.HResult = 404;
        }
    }
}
