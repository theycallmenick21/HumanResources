namespace HumanResources.Shared.DTOs.Location
{
    public class LocationUpdateDto
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        public int CityId { get; set; }
    }
}
