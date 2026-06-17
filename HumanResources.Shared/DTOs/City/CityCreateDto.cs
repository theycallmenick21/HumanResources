namespace HumanResources.Shared.DTOs.City
{
    public class CityCreateDto
    {
        public required string Name { get; set; }
        public int CountryId { get; set; }
    }
}