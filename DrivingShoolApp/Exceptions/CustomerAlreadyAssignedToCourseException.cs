namespace DrivingSchoolApp.Exceptions
{
    public class CustomerAlreadyAssignedToCourseException : DataInconsistencyException
    {
        public CustomerAlreadyAssignedToCourseException(int customerId, int courseId) : base(String.Format("There is customer with id {0} already assgined to course with id {1}.", customerId, courseId))
        {
            this.HResult = 409;
        }
    }
}

