namespace HumanResources.Shared.DTOs.Department
{
    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int LocationId { get; set; }
    }
}
