namespace HumanResources.Shared.DTOs.City
{
    public class CityUpdateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int CountryId { get; set; }
    }
}
