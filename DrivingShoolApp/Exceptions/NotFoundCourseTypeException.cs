namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundCourseTypeException : DataInconsistencyException
    {
        public NotFoundCourseTypeException() : base(String.Format("Not found any course types."))
        {
            this.HResult = 404;
        }
        public NotFoundCourseTypeException(int courseTypeId) : base(String.Format("Not found course type with id {0}.", courseTypeId))
        {
            this.HResult = 404;
        }
    }
}
