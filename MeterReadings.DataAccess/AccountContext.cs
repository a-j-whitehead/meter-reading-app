using MeterReadings.Domain;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.DataAccess
{
    public class AccountContext : DbContext
    {
        private const string _localSqlServerName = "localhost\\SQLEXPRESS";
        public DbSet<Account> Accounts { get; set; }

        public AccountContext() { }

        public void InitialSetup()
        {
            if (Database.EnsureCreated())
            {
                Accounts.AddRange(
                [
                    new Account(2344, "Tommy", "Test"),
                    new Account(2233, "Barry", "Test"),
                    new Account(8766, "Sally", "Test"),
                    new Account(2345, "Jerry", "Test"),
                    new Account(2346, "Ollie", "Test"),
                    new Account(2347, "Tara", "Test"),
                    new Account(2348, "Tammy", "Test"),
                    new Account(2349, "Simon", "Test"),
                    new Account(2350, "Colin", "Test"),
                    new Account(2351, "Gladys", "Test"),
                    new Account(2352, "Greg", "Test"),
                    new Account(2353, "Tony", "Test"),
                    new Account(2355, "Arthur", "Test"),
                    new Account(2356, "Craig", "Test"),
                    new Account(6776, "Laura", "Test"),
                    new Account(4534, "JOSH", "TEST"),
                    new Account(1234, "Freya", "Test"),
                    new Account(1239, "Noddy", "Test"),
                    new Account(1240, "Archie", "Test"),
                    new Account(1241, "Lara", "Test"),
                    new Account(1242, "Tim", "Test"),
                    new Account(1243, "Graham", "Test"),
                    new Account(1244, "Tony", "Test"),
                    new Account(1245, "Neville", "Test"),
                    new Account(1246, "Jo", "Test"),
                    new Account(1247, "Jim", "Test"),
                    new Account(1248, "Pam", "Test"),
                ]);

                SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer($"Server={_localSqlServerName};Database=MeterReadings;Trusted_Connection=True;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SetUpAccount(modelBuilder);

            SetUpMeterReading(modelBuilder);
        }

        private static void SetUpAccount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .ToTable("Account");

            modelBuilder.Entity<Account>()
                .Property(x => x.FirstName);

            modelBuilder.Entity<Account>()
                .Property(x => x.LastName);

            modelBuilder.Entity<Account>()
                .HasKey(x => x.AccountId);

            modelBuilder.Entity<Account>()
                .HasMany(x => x.MeterReadings)
                .WithOne(x => x.Account)
                .HasPrincipalKey(x => x.AccountId);
        }

        private static void SetUpMeterReading(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeterReading>()
                .HasKey(x => x.MeterReadingId);

            modelBuilder.Entity<MeterReading>()
                .Property(x => x.TimeOfMeterReading);

            modelBuilder.Entity<MeterReading>()
                .Property(x => x.Value);

            modelBuilder.Entity<MeterReading>()
                .HasOne(x => x.Account)
                .WithMany(x => x.MeterReadings)
                .HasForeignKey(x => x.AccountId);
        }
    }
}
