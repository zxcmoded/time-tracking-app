namespace tracking_app.Dto
{
    public class TimeLogDto
    {
        public int? LogId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime DateCreated { get; set; }
        public double TotalHours { get; set; }
    }
}