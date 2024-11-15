namespace DataAccess.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string SurnameName { get; set; }
        public virtual ICollection<MeterReading>? MeterReadings { get; set; }
    }
}
