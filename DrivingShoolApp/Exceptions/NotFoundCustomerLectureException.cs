namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomerLectureException : DataInconsistencyException
    {
        public NotFoundCustomerLectureException() : base(String.Format("Not found any lectures attended by customers."))
        {
            this.HResult = 404;
        }

        public NotFoundCustomerLectureException(int customerId) : base(String.Format("Customer with id {0} didn't attend any lecture.", customerId))
        {
            this.HResult = 404;
        }

        public NotFoundCustomerLectureException(int customerId, int lectureId) : base(String.Format("Customer with id {0} didn't attend  lecture with id {1}.", customerId, lectureId))
        {
            this.HResult = 404;
        }
    }
}
