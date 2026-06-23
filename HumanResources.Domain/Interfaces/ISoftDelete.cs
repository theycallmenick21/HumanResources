namespace HumanResources.Domain.Interfaces
{
    public interface ISoftDelete
    {
        bool IsActive { get; set; }
    }
}