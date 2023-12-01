namespace DrivingSchoolApp.DTOs
{
    public class SubjectResponseDTO
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int Duration { get; set; }

    }
}
