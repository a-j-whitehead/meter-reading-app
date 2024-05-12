namespace MeterReadings.Domain
{
    public record MeterReading
    {
        public int MeterReadingId { get; }
        public DateTimeOffset TimeOfMeterReading { get; }
        public string Value { get; }

        public int AccountId { get; }
        public Account Account { get; }

        public MeterReading() { }

        internal MeterReading(Account account, DateTimeOffset timeOfMeterReading, string value)
        {
            Account = account;
            TimeOfMeterReading = timeOfMeterReading;
            Value = value;
        }
    }
}
