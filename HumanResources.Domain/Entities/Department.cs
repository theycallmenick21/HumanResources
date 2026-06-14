namespace HumanResources.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
    }
}
