namespace HumanResources.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
    }
}
