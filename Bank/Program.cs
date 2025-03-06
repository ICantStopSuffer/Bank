using Bank.Modules;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Bank
{

    public class ApplicationContext : DbContext {
        public DbSet<BankAccount> accounts { get; set; } = null!;

        public ApplicationContext() {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345");
        }
    }

    public class InsufficientFundsError : Exception {
        public InsufficientFundsError() { }
        public InsufficientFundsError(string msg) : base(msg) { }
    }
    public class InvalidAmountError : Exception {
        public InvalidAmountError() { }
        public InvalidAmountError(string msg) : base(msg) { }
    }
    public class AccountNotFoundError : Exception {
        public AccountNotFoundError() { }
        public AccountNotFoundError(string msg) : base(msg) { }
    }

    public class Program {


        static void Main(string[] args) {
            ApplicationContext context = new ApplicationContext();
            TransactionManager manager = new TransactionManager();
            BankAccount account0 = new BankAccount();
            BankAccount account1 = new BankAccount();

            account0.deposit(1000);
            account1.deposit(1000);

            string choise = "";

            while (choise != "exit") {
                Console.WriteLine(@"1 - Add balance to account
2 - Withdraw from account
3 - Transfer
4 - Show accounts
write 'exit' to exit");
                choise = Console.ReadLine()!;

                try {
                    if (choise == "1") {
                        Console.WriteLine("write account id");
                        int id = Int32.Parse(Console.ReadLine()!);
                        Console.WriteLine("write amount");
                        double amount = Double.Parse(Console.ReadLine()!);

                        if (!BankAccount.accounts.ContainsKey(id)) {
                            throw Logger.logException(new AccountNotFoundError("Аккаунт не найден"));
                        }

                        BankAccount.accounts[id].deposit(amount);
                    } else if (choise == "2") {
                        Console.WriteLine("write account id");
                        int id = Int32.Parse(Console.ReadLine()!);
                        Console.WriteLine("write amount");
                        double amount = Double.Parse(Console.ReadLine()!);

                        if (!BankAccount.accounts.ContainsKey(id)) {
                            throw Logger.logException(new AccountNotFoundError("Аккаунт не найден"));
                        }

                        BankAccount.accounts[id].withdraw(amount);
                    } else if (choise == "4") {
                        foreach (BankAccount account in BankAccount.accounts.Values) {
                            Console.WriteLine(account.ToString());
                        }
                    } else if (choise == "3") {
                        Console.WriteLine("write first account id");
                        int id1 = Int32.Parse(Console.ReadLine()!);
                        Console.WriteLine("write second account id");
                        int id2 = Int32.Parse(Console.ReadLine()!);
                        Console.WriteLine("write amount");
                        double amount = Double.Parse(Console.ReadLine()!);

                        manager.transfer(id1, id2, amount);
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}