namespace HumanResources.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public int FirstName { get; set; }
        public int LastName { get; set; }
        public int Email { get; set; }
        public int HiringDate { get; set; }
        public int Salary { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public Role? Role { get; set; }
        public Department? Department { get; set; }

    }
}
