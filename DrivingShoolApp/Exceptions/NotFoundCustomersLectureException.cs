namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomersLectureException : DataInconsistencyException
    {
        public NotFoundCustomersLectureException(int lectureId) : base(String.Format("There wasn't any customer at lecture with id {0}.", lectureId))
        {
            this.HResult = 404;
        }
    }
}
