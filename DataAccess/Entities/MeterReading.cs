namespace DataAccess.Entities
{
    public class MeterReading
    {
        public int Id { get; set; }
        public required DateTime ReadingDateTime { get; set; }
        public required string ReadValue { get; set; }
        public required int AccountId { get; set; }
        public virtual Account? Account { get; set; }
    }
}
