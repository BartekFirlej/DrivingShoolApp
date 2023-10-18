namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCustomersLecturesException : DataInconsistencyException
    {
        public NotFoundCustomersLecturesException() : base(String.Format("Not found any lectures attended by customers."))
        {
            this.HResult = 404;
        }
    }
}
