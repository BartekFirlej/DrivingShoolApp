namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomerLectureException : DataInconsistencyException
    {
        public NotFoundCustomerLectureException(int customerId, int lectureId) : base(String.Format("Customer with id {0} didn't attend  lecture with id {1}.", customerId, lectureId))
        {
            this.HResult = 404;
        }
    }
}
