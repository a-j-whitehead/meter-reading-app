using MeterReadings.Domain;

namespace MeterReadings.Tests
{
    public class AccountTests
    {
        [Theory]
        [InlineData("22/04/2019 12:25", "12345")]
        public void Should_Accept_Valid_Meter_Readings(string? timeOfMeterReading, string? meterReadingValue)
        {
            var account = new Account();

            var meterReadingWasAdded = account.AddMeterReading(timeOfMeterReading, meterReadingValue);

            Assert.True(meterReadingWasAdded);
        }

        [Theory]
        [InlineData("01/22/2019 12:25", "12345")]
        [InlineData("01/02/2019 89:25", "12345")]
        [InlineData("not-a-date", "12345")]
        [InlineData("01/02/2019 10:25", "0a786")]
        [InlineData("01/02/2019 10:25", "1234")]
        [InlineData("01/02/2019 10:25", "123456")]
        [InlineData("01/02/2019 10:25", "34 89")]
        [InlineData("01/02/2019 10:25", "34+79")]
        [InlineData("01/02/2019 10:25", null)]
        [InlineData(null, "12345")]
        public void Should_Not_Accept_Invalid_Meter_Readings(string? timeOfMeterReading, string? meterReadingValue)
        {
            var account = new Account();

            var meterReadingWasAdded = account.AddMeterReading(timeOfMeterReading, meterReadingValue);

            Assert.False(meterReadingWasAdded);
        }

        [Fact]
        public void Should_Not_Accept_Old_Meter_Readings()
        {
            var account = new Account();
            account.AddMeterReading("01/02/2019 10:25", "12345");

            var meterReadingWasAdded = account.AddMeterReading("01/01/2019 10:25", "12345");

            Assert.False(meterReadingWasAdded);
        }

        [Fact]
        public void Should_Accept_Multiple_Meter_Readings_In_Correct_Chronological_Order()
        {
            var account = new Account();
            account.AddMeterReading("01/02/2019 10:25", "12345");

            var meterReadingWasAdded = account.AddMeterReading("01/03/2019 10:25", "12345");

            Assert.True(meterReadingWasAdded);
        }

        [Fact]
        public void Should_Not_Send_Same_Meter_Reading_Twice()
        {
            var account = new Account();
            account.AddMeterReading("01/02/2019 10:25", "12345");

            var meterReadingWasAdded = account.AddMeterReading("01/02/2019 10:25", "12345");

            Assert.False(meterReadingWasAdded);
        }
    }
}