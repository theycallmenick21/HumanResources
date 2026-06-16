using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;

namespace HumanResources.Application.Services
{
    public class RecordService : GenericService<Record>, IRecordService
    {
        public RecordService(IGenericRepository<Record> repository) : base(repository)
        {
        }
    }
}
