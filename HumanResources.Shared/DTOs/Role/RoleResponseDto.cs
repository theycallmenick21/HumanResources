namespace HumanResources.Shared.DTOs.Role
{
    public class RoleResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
    }
}
