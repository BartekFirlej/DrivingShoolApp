namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundRegistrationException : DataInconsistencyException
    {
        public NotFoundRegistrationException() : base(String.Format("Not found any registrations."))
        {
            this.HResult = 404;
        }
        public NotFoundRegistrationException(int customerId) : base(String.Format("Not found any registrations for customer with id {0}.", customerId))
        {
            this.HResult = 404;
        }

        public NotFoundRegistrationException(int customerId, int courseId) : base(String.Format("Not found registration from customer with id {0} for course with id {1}.", customerId, courseId))
        {
            this.HResult = 404;
        }
    }
}
