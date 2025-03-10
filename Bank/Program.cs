using Bank.Modules;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Security.Cryptography;

namespace Bank
{

    public class Program {

        public static string hashPassword(string password) {
            byte[] salt = { 11, 12, 13, 14, 15, 16, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[20];
            Array.Copy(hash, 0, hashBytes, 0, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            return savedPasswordHash;
        }

        public static Database context = new Database();
        public static TransactionManager manager = new TransactionManager();

        static void Main(string[] args) {
            if (context.accounts.ToList().Count <= 0) {
                for (int i = 0; i < 10; i++) {
                    BankAccount account = new BankAccount();
                    account.deposit(new Random().NextDouble() * 100);

                    string email = "";
                    for (int c = 0; c < 10; c++) {
                        email += (char)(new Random().Next(66, 123));
                    }
                    account.setEmail(email + "@gmail.com");
                    account.setPassword(email);

                    context.accounts.Add(account);
                }
                context.SaveChanges();
            }

            string choise = "";
            while (choise != "exit") {
                Console.WriteLine(@"1 - Add balance to account
2 - Withdraw from account
3 - Transfer
4 - Show accounts
5 - Save accounts to file
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

                    } else if (choise == "3") {
                        Console.WriteLine("write first account id");
                        int id1 = Int32.Parse(Console.ReadLine()!);
                        Console.WriteLine("write second account id");
                        int id2 = Int32.Parse(Console.ReadLine()!);
                        Console.WriteLine("write amount");
                        double amount = Double.Parse(Console.ReadLine()!);

                        manager.transfer(id1, id2, amount);
                        context.SaveChanges();
                    } else if (choise == "4") {
                        foreach (BankAccount account in context.accounts) {
                            Console.WriteLine(account.ToString());
                        }
                    } else if (choise == "5") {
                        context.saveToFile();
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}