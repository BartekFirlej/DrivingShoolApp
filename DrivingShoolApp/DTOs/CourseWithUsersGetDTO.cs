namespace DrivingSchoolApp.DTOs
{
    public class CourseWithUsersGetDTO
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public DateTime BeginDate {  get; set; }
        public int Limit {  get; set; }
        public int AssignedUserseCount {  get; set; }
        public List<CustomerGetDTO> AssignedUsers { get; set; }
    }
}
