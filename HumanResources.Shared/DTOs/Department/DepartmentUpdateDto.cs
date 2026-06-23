namespace HumanResources.Shared.DTOs.Department
{
    public class DepartmentUpdateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int LocationId { get; set; }
    }
}
