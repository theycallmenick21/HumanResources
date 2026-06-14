namespace HumanResources.Domain.Entities
{
    public class Record
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public Employee? Employee { get; set; }
        public Role? Role { get; set; }
        public Department? Department { get; set; }
    }
}