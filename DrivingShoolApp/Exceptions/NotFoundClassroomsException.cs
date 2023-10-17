namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundClassroomsException : DataInconsistencyException
    {
        public NotFoundClassroomsException() : base(String.Format("Not found any classrooms."))
        {
            this.HResult = 404;
        }
    }
}
