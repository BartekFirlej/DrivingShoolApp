namespace DrivingSchoolApp.DTOs
{
    public class CourseSubjectsGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BeginDate { get; set; }
        public float Price { get; set; }
        public int Limit { get; set; }
        public int CourseTypeId { get; set; }
        public string CourseTypeName {  get; set; }
        public List<CourseSubjectSequenceGetDTO> CourseSubjects {  get; set; }
    }
}
