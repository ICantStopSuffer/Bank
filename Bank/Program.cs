using Bank.Modules;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Bank
{

    public class DataBase : DbContext {
        public DbSet<BankAccount> accounts { get; set; } = null!;

        public DataBase() {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345");
        }
    }

    public class Program {

        public static DataBase context = new DataBase();

        static void Main(string[] args) {
            TransactionManager manager = new TransactionManager();

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

                        BankAccount? account = context.accounts.FirstOrDefault(account => account.id == id);

                        if (account == null) {
                            throw Logger.logException(new AccountNotFoundError("Аккаунт не найден"));
                        }

                        account.deposit(amount);
                        context.SaveChanges();

                    } else if (choise == "2") {
                        Console.WriteLine("write account id");
                        int id = Int32.Parse(Console.ReadLine()!);
                        Console.WriteLine("write amount");
                        double amount = Double.Parse(Console.ReadLine()!);

                        BankAccount? account = context.accounts.FirstOrDefault(account => account.id == id);

                        if (account == null) {
                            throw Logger.logException(new AccountNotFoundError("Аккаунт не найден"));
                        }

                        account.withdraw(amount);
                        context.SaveChanges();

                    } else if (choise == "4") {
                        foreach (BankAccount account in context.accounts) {
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
                        context.SaveChanges();
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}