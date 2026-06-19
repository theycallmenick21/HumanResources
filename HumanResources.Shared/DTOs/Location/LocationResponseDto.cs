namespace HumanResources.Shared.DTOs.Location
{
    public class LocationResponseDto
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        public int CityId { get; set; }
    }
}
