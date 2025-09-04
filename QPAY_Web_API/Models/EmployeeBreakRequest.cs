namespace QPay.API.Models
{
    public class EmployeeBreakRequest
    {
        
        public int userId { get; set; }
        public DateTime date { get; set; }
    }

    public class EmployeeBreakBulkModelRequest
    {
        public int userId { get; set; } = 0;
        public List<EmployeeBreakModelRequest>? employeeBreakRequest {  get; set; } 
    }

    public class EmployeeBreakModelRequest
    {

        public int? userId { get; set; }
        public int? breakId { get; set; }
        public int? userBreakId { get; set; }
        public int?  breakTypeId { get; set; }
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public string? Remarks { get; set; }
        public string? description { get; set; }
        
        

    }
}
