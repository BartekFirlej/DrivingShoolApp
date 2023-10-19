namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomerException : DataInconsistencyException
    {
        public NotFoundCustomerException() : base(String.Format("Not found any customer."))
        {
            this.HResult = 404;
        }
        public NotFoundCustomerException(int customerId) : base(String.Format("Not found customer with id {0}.", customerId))
        {
            this.HResult = 404;
        }
    }
}
