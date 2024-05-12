namespace MeterReadings.Domain
{
    public interface IAccountRepository
    {
        Task<ICollection<Account>> Get(IEnumerable<int> accountIds);
        Task Save();
    }
}
