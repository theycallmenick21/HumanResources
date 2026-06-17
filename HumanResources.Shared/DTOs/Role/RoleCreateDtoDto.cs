namespace HumanResources.Shared.DTOs.Role
{
    public class RoleCreateDtoDto
    {
        public required string Name { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
    }
}
