namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundClassroomException : DataInconsistencyException
    {
        public NotFoundClassroomException(int classroomId) : base(String.Format("Not found address with id {0}.", classroomId))
        {
            this.HResult = 404;
        }
    }
}
