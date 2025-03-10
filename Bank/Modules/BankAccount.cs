using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
namespace Bank.Modules
{
    public class BankAccount
    {
        //public static Dictionary<int, BankAccount> accounts = new Dictionary<int, BankAccount>();
        private static int nextId;
        public int id { private set; get; }
        public string email { private set; get; } = "";
        public string password { private set; get; } = "";
        public double balance { private set; get; }

        public BankAccount()
        {
            id = ++nextId;
        }

        public void withdraw(double amount)
        {
            if (amount > balance)
            {
                throw Logger.logException(new InsufficientFundsError("Количество привышает баланс"));
            }
            if (amount < 0)
            {
                throw Logger.logException(new InvalidAmountError("Количество должно быть положительным"));
            }

            balance -= amount;
        }

        public void deposit(double amount)
        {
            if (amount < 0)
            {
                throw Logger.logException(new InvalidAmountError("Количество должно быть положительным"));
            }

            balance += amount;
        }

        public void setEmail(string email) {
            string domain = email.Split('@')[1];

            if (domain == null) {
                throw new InvalidEmailError("Invalid email, add domain");
            }

            this.email = email;
        }

        public void setPassword(string password) {
            this.password = Program.hashPassword(password);
        }

        public override string ToString()
        {
            string result = "";

            foreach (var field in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)) {
                result += $"{field.Name}: {field.GetValue(this)} | ";
            }

            return result;
        }
    }
}