namespace HumanResources.Shared.DTOs.Department
{
    public class DepartmentCreateDto
    {
        public required string Name { get; set; }
        public int LocationId { get; set; }
    }
}
