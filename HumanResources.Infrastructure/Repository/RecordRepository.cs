using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;
using HumanResources.Infrastructure.Data;

namespace HumanResources.Infrastructure.Repository
{
    public class RecordRepository : GenericRepository<Record>, IRecordRepository
    {
        public RecordRepository(HumanResourcesContext context) : base(context)
        {
        }
    }
}
