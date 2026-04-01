namespace QPay.API.Models
{
    public class DashboardModelRequest
    {
        public int userId {  get; set; }
        public string flag {  get; set; }
        public DateTime? fromDate {  get; set; }
        public DateTime? toDate { get; set;}
    }
}
