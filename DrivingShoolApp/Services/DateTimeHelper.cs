namespace DrivingSchoolApp.Services
{
    public interface IDateTimeHelper
    {
        DateTime GetDateTimeNow();
    }

    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
    }
}
