namespace DVisit.Models
{
    public class TransactionItem
    {
        public DateTime? DateAndTime { get; set; }
        public VisitorItem? Visitor { get; set; }
        public bool Allowed { get; set; }
    }
}
