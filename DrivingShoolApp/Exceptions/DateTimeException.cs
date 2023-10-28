namespace DrivingSchoolApp.Exceptions
{
    public class DateTimeException : DataInconsistencyException
    {
        public DateTimeException(string name) : base(String.Format("Not found {0} datetime.", name))
        {
            this.HResult = 400;
        }
        public DateTimeException(string firstDate, string secondDate) : base(String.Format("{0} datetime is greater than {1} datetime.", firstDate, secondDate))
        {
            this.HResult = 400;
        }
    }
}
