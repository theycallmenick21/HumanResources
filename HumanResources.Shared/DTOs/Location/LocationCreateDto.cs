using HumanResources.Shared.DTOs.City;

namespace HumanResources.Shared.DTOs.Location
{
    public class LocationCreateDto
    {
        public required string Address { get; set; }
        public int CityId { get; set; }
    }
}
