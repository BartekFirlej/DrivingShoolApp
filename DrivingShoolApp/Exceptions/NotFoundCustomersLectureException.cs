namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomersLectureException : DataInconsistencyException
    {
        public NotFoundCustomersLectureException(int lectureId) : base(String.Format("Lecture with id {0} wasn't attended by any customers.", lectureId))
        {
            this.HResult = 404;
        }
    }
}
