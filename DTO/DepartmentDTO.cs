namespace LabApi.DTO
{
    public class DepartmentDTO
    {
        public string DeptName { get; set; } = null!;
        public List<string> StudentSNames { get; set; } = null!;
        public string ManagerName { get; set; } = null!;
        public int Count { get; set; } 
        public String Message { get; set; }=null!;
    }
}
