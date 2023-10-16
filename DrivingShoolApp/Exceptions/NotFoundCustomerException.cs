using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomerException : DataInconsistencyException
    {
        public NotFoundCustomerException(int customerId) : base(String.Format("Not found customer with id {0}.", customerId))
        {
            this.HResult = 404;
        }
    }
}
