namespace ExamCalendar.Web.Exceptions
{
    public class DataInconsistencyException : Exception
    {
        public DataInconsistencyException() : base("Dane nieprawidłowe.")
        {
            this.HResult = 404;
        }
        public DataInconsistencyException(string message) : base(message) {}
        public Dictionary<string, object> ToJson()
        {
            return new Dictionary<string, object>()
            {
                {"Type", this.HResult},
                {"Error", this.Message}
            };
        }
    }
}
