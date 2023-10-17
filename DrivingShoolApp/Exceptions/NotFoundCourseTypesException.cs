namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCourseTypesException : DataInconsistencyException
    {
        public NotFoundCourseTypesException() : base(String.Format("Not found any course types."))
        {
            this.HResult = 404;
        }
    }
}
