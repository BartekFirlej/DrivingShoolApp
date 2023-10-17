namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomersException : DataInconsistencyException
    {
        public NotFoundCustomersException() : base(String.Format("Not found any customer."))
        {
            this.HResult = 404;
        }
    }
}
