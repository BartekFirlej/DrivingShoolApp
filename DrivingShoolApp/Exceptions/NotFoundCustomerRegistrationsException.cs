namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomerRegistrationsException : DataInconsistencyException
    {
        public NotFoundCustomerRegistrationsException(int customerId) : base(String.Format("Not found any registrations for customer with id {0}.", customerId))
        {
            this.HResult = 404;
        }
    }
}
