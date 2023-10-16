using ExamCalendar.Web.Exceptions;

namespace DrivingSchoolApp.Exceptions
{
    public class NotFoundSubjectException : DataInconsistencyException
    {
        public NotFoundSubjectException(int subjectId) : base(String.Format("Not found subject with id {0}.", subjectId))
        {
            this.HResult = 404;
        }
    }
}
