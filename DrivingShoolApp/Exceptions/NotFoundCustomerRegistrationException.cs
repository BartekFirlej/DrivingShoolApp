using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomerRegistrationException : DataInconsistencyException
    {
        public NotFoundCustomerRegistrationException(int customerId) : base(String.Format("Not found any registrations for customer with id {0}.", customerId))
        {
            this.HResult = 404;
        }
    }
}
