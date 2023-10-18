namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundLecturesException : DataInconsistencyException
    {
        public NotFoundLecturesException() : base(String.Format("Not found any lecture."))
        {
            this.HResult = 404;
        }
    }
}
