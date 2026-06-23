using HumanResources.Application.Interfaces;
using HumanResources.Domain.Entities;
using HumanResources.Domain.Interfaces;
using HumanResources.Shared.Wrappers;

namespace HumanResources.Application.Services
{
    public class EmployeeService : GenericService<Employee>, IEmployeeService
    {
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Record> _recordRepository;

        public EmployeeService(
            IGenericRepository<Employee> employeeRepository,
            IGenericRepository<Record> recordRepository,
            IGenericRepository<Employee> repository) : base(repository)
        {
            _employeeRepository = employeeRepository;
            _recordRepository = recordRepository;
        }

        public async Task<Response<bool>> InsertEmployeeAsync(Employee employee)
        {
            Response<bool> response = await _employeeRepository.InsertAsync(employee);

            if (response.IsSuccess)
            {
                Record newRecord = new()
                {
                    EmployeeId = employee.Id,
                    RoleId = employee.RoleId,
                    DepartmentId = employee.DepartmentId,
                    StartDate = DateTime.Today
                };

                await _recordRepository.InsertAsync(newRecord);
            }

            return response;
        }

        public async Task<Response<bool>> UpdateEmployeeAsync(Employee employee)
        {
            var response = await _employeeRepository.UpdateAsync(employee);

            if (response.IsSuccess)
            {
                Record newRecord = new()
                {
                    EmployeeId = employee.Id,
                    RoleId = employee.RoleId,
                    DepartmentId = employee.DepartmentId,
                    StartDate = DateTime.Today
                };

                await _recordRepository.InsertAsync(newRecord);
            }

            return response;
        }

        public async Task<Response<bool>> DeleteEmployeeAsync(int id)
        {
            var employeeResponse = await _employeeRepository.GetByIdAsync(id);

            var response = await _employeeRepository.DeleteAsync(id);

            if (response.IsSuccess && employeeResponse.IsSuccess && employeeResponse.Data != null)
            {
                Record terminationRecord = new()
                {
                    EmployeeId = id,
                    RoleId = employeeResponse.Data.RoleId,
                    DepartmentId = employeeResponse.Data.DepartmentId,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today
                };

                await _recordRepository.InsertAsync(terminationRecord);
            }

            return response;
        }
    }
}