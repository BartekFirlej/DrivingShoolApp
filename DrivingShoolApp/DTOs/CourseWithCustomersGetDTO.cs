namespace DrivingSchoolApp.DTOs
{
    public class CourseWithCustomersGetDTO
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public DateTime BeginDate {  get; set; }
        public int Limit {  get; set; }
        public int AssignedCustomersCount {  get; set; }
        public List<CustomerGetDTO> AssignedCustomersList { get; set; }
    }
}
