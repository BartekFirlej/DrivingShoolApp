namespace DrivingSchoolApp.Exceptions
{
    public class AssignLimitReachedException : DataInconsistencyException
    {
        public AssignLimitReachedException(int courseId) : base(String.Format("There is reached assign limit for course with id {0}.", courseId))
        {
            this.HResult = 409;
        }
    }
}
