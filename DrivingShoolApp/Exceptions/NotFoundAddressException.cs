namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundAddressException : DataInconsistencyException
    {
        public NotFoundAddressException() : base(String.Format("Not found any addresses."))
        {
            this.HResult = 404;
        }
        public NotFoundAddressException(int addressId) : base(String.Format("Not found address with id {0}.", addressId))
        {
            this.HResult = 404;
        }
    }
}
