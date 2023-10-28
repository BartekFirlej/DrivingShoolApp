namespace DrivingSchoolApp.Exceptions
{
    public class WrongPostalCodeFormatException : DataInconsistencyException
    {
        public WrongPostalCodeFormatException(string postalCode) : base(String.Format("Your postal code {0} doesn't match pattern.",postalCode))
        {
            this.HResult = 400;
        }
    }
}
