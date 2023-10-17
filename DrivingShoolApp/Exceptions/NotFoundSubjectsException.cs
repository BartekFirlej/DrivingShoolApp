namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundSubjectsException : DataInconsistencyException
    {
        public NotFoundSubjectsException() : base("Not found any subjects.")
        {
            this.HResult = 404;
        }
    }
}
