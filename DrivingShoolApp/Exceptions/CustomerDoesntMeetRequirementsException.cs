namespace DrivingSchoolApp.Exceptions
{
    public class CustomerDoesntMeetRequirementsException : DataInconsistencyException
    {
        public CustomerDoesntMeetRequirementsException(int customerId, int licenceCategoryId) :
            base(String.Format("Customer with id {0} doesn't meet requirements to get driving licence category {1}.", customerId, licenceCategoryId))
        {
            this.HResult = 409;
        }
        public CustomerDoesntMeetRequirementsException(int customerId) :
            base(String.Format("Customer with id {0} doesn't meet age requirement for this course.", customerId))
        {
            this.HResult = 409;
        }
    }
}
