namespace DrivingSchoolApp.DTOs
{
    public class ClassroomGetDTO
    {
        public int ClassroomId { get; set; }
        public int ClassroomNumber {  get; set; }
        public int Size { get; set; }
        public AddressGetDTO Address { get; set; }
    }
}
