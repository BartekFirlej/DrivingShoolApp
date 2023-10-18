namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundAddressesException : DataInconsistencyException
    {
        public NotFoundAddressesException() : base(String.Format("Not found any addresses."))
        {
            this.HResult = 404;
        }
    }
}
