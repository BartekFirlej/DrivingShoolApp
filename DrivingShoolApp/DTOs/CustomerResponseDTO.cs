namespace DrivingSchoolApp.DTOs
{
    public class CustomerResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string SecondName { get; set; } = null!;

        public DateTime BirthDate { get; set; }
    }
}
