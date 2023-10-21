namespace DrivingSchoolApp.Exceptions
{
    public class CustomerAlreadyAssignedToLectureException : DataInconsistencyException
    {
        public CustomerAlreadyAssignedToLectureException(int customerId, int lectureId) : base(String.Format("There is customer with id {0} already assgined to lecture with id {1}.", customerId, lectureId))
        {
            this.HResult = 409;
        }
    }
}

