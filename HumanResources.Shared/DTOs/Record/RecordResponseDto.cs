namespace HumanResources.Shared.DTOs.Record
{
    public class RecordResponseDto
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
    }
}
