using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundLicenceCategoriesException : DataInconsistencyException
    {
        public NotFoundLicenceCategoriesException() : base(String.Format("Not found any licence category."))
        {
            this.HResult = 404;
        }
    }
}
