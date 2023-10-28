namespace DrivingSchoolApp.Exceptions
{
    public class ValueMustBeGreaterThanZeroException : DataInconsistencyException
    {
        public ValueMustBeGreaterThanZeroException(string name) : base(String.Format("Value of {0} must be greater than 0", name))
        {
            this.HResult = 400;
        }
    }
}
