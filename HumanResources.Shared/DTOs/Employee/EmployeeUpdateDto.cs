namespace HumanResources.Shared.DTOs.Employee
{
    public class EmployeeUpdateDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public DateTime HiringDate { get; set; }
        public decimal Salary { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
    }
}
