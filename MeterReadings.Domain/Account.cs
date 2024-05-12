using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeterReadings.Domain
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountId { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public ICollection<MeterReading> MeterReadings { get; } = [];

        public Account() { }

        public Account(int accountId, string firstName, string lastName)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
        }

        public bool AddMeterReading(string? timeOfMeterReading, string? meterReadingValue)
        {
            if (!DateTimeOffset.TryParse(timeOfMeterReading, out var convertedTimeOfMeterReading))
            {
                return false;
            }
            if (MeterReadingIsNotLatest(convertedTimeOfMeterReading))
            {
                return false;
            }
            if (!MeterReadingValueIsValid(meterReadingValue))
            {
                return false;
            }

            MeterReadings.Add(new MeterReading(this, convertedTimeOfMeterReading, meterReadingValue!));
            return true;
        }

        private bool MeterReadingIsNotLatest(DateTimeOffset timeOfMeterReading)
        {
            return MeterReadings.Any(x => DateTimeOffset.Compare(x.TimeOfMeterReading, timeOfMeterReading) >= 0);
        }

        private static bool MeterReadingValueIsValid(string? meterReadingValue)
        {
            return meterReadingValue != null
                && meterReadingValue.Length == 5
                && meterReadingValue.All(char.IsNumber);
        }
    }
}
