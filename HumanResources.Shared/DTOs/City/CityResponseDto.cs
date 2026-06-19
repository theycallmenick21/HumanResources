namespace HumanResources.Shared.DTOs.City
{
    public class CityResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int CountryId { get; set; }
    }
}