using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Modules {
    public class Database : DbContext {
        public DbSet<BankAccount> accounts { get; set; } = null!;

        public Database() {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345");
        }

        public void saveToFile() {

            File.WriteAllText("accounts.txt", "");

            foreach (BankAccount account in accounts) {
                Console.WriteLine(account);
                File.AppendAllText("accounts.txt", account.ToString() + "\n");
            }
        }
    }

}
