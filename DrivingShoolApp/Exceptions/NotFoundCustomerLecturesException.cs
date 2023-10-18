namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomerLecturesException : DataInconsistencyException
    {
        public NotFoundCustomerLecturesException(int customerId) : base(String.Format("Customer with id {0} didn't attend any lecture.", customerId))
        {
            this.HResult = 404;
        }
    }
}
