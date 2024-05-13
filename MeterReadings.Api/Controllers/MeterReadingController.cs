using CsvHelper;
using MeterReadings.DataAccess;
using MeterReadings.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Api.Controllers
{
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public MeterReadingController()
        {
            _accountRepository = new AccountRepository(new AccountContext());
        }

        [HttpPost("submit-meter-readings")]
        public async Task<IActionResult> SubmitMeterReadings([FromForm] IFormFile meter_readings)
        {
            var meterReadings = ReadMeterReadingsFromFile(meter_readings);

            var accounts = await _accountRepository.Get(meterReadings.Select(x => x.AccountId ?? 0));

            var successfulReadings = 0;
            var failedReadings = 0;

            foreach (var meterReading in meterReadings)
            {
                var account = accounts.SingleOrDefault(x => x.AccountId == meterReading.AccountId);
                if (account == null)
                {
                    failedReadings++;
                }
                else
                {
                    if (account.AddMeterReading(meterReading.MeterReadingDateTime, meterReading.MeterReadValue))
                    {
                        successfulReadings++;
                    }
                    else
                    {
                        failedReadings++;
                    }
                }
            }
            await _accountRepository.Save();

            return Ok($"Meter readings successfully saved: {successfulReadings}; meter readings not saved: {failedReadings}");
        }

        private static List<MeterReadings.Api.Models.MeterReading> ReadMeterReadingsFromFile(IFormFile meter_readings)
        {
            var reader = new StreamReader(meter_readings.OpenReadStream());
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<MeterReadings.Api.Models.MeterReading>().ToList();
        }
    }
}
