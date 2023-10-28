namespace DrivingSchoolApp.Exceptions
{
    public class TakenSequenceNumberException : DataInconsistencyException
    {
        public TakenSequenceNumberException(int courseId, int sequenceNumber) : base(String.Format("There is sequence number {0} already assgined to subject in course with id {1}.", sequenceNumber, courseId))
        {
            this.HResult = 409;
        }
    }
}
