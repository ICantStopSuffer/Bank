using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Modules
{
    public class BankAccount
    {
        public static Dictionary<int, BankAccount> accounts = new Dictionary<int, BankAccount>();
        private static int nextId;
        public int id { private set; get; }
        public double balance { private set; get; }

        public BankAccount()
        {
            id = nextId++;
            accounts.Add(id, this);
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

        public override string ToString()
        {
            return $"{id}: {balance}";
        }
    }
}
