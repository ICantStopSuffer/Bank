using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
namespace Bank.Modules
{
    public enum valueType {
        RUB,
        USD,
        EUR,
    }

    public class BankAccount
    {
        //public static Dictionary<int, BankAccount> accounts = new Dictionary<int, BankAccount>();
        private static int nextId;
        public int id { private set; get; }
        public string email { private set; get; } = "";
        public string password { private set; get; } = "";
        public double balance { private set; get; }
        public double balance_usd { private set; get; }
        public double balance_eur { private set; get; }

        public BankAccount()
        {
            id = ++nextId;
        }

        public void withdraw(double amount, valueType type = valueType.RUB)
        {
            if (amount < 0)
            {
                throw Logger.logException(new InvalidAmountError("Количество должно быть положительным"));
            }

            switch (type) {
                case valueType.RUB:
                    if (amount > balance) {
                        throw Logger.logException(new InsufficientFundsError("Количество привышает баланс"));
                    }
                    balance -= amount;
                    break;

                case valueType.USD:
                    if (amount > balance_usd) {
                        throw Logger.logException(new InsufficientFundsError("Количество привышает баланс"));
                    }
                    balance_usd -= amount;
                    break;

                case valueType.EUR:
                    if (amount > balance_eur) {
                        throw Logger.logException(new InsufficientFundsError("Количество привышает баланс"));
                    }
                    balance_eur -= amount;
                    break;
            }
        }

        public void deposit(double amount, valueType type = valueType.RUB)
        {
            if (amount < 0)
            {
                throw Logger.logException(new InvalidAmountError("Количество должно быть положительным"));
            }

            switch (type) {
                case valueType.RUB:
                    balance += amount;
                    break;

                case valueType.USD:
                    balance_usd += amount;
                    break;

                case valueType.EUR:
                    balance_eur += amount;
                    break;
            }
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
                string name = field.Name;
                int index = name.IndexOf(">");

                if (index != -1) {
                    name = name.Substring(1, index - 1);
                }

                result += $"{name}: {field.GetValue(this)} | ";
            }

            return result;
        }
    }
}