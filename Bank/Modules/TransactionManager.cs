using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Modules
{
    public class TransactionManager
    {
        public TransactionManager() { }

        public void transfer(BankAccount? account0, BankAccount? account1, double amount, valueType type = valueType.RUB)
        {
            if (account0 == null || account1 == null)
            {
                throw Logger.logException(new AccountNotFoundError("Аккаунт не найден"));
            }

            account0.withdraw(amount, type);
            account1.deposit(amount, type);
        }

        public void transfer(int accountId0, int accountId1, double amount, valueType type = valueType.RUB)
        {
            BankAccount? account0 = Program.context.accounts.FirstOrDefault(account => account.id == accountId0);
            BankAccount? account1 = Program.context.accounts.FirstOrDefault(account => account.id == accountId1);

            /*if (!BankAccount.accounts.ContainsKey(accountId0) || !BankAccount.accounts.ContainsKey(accountId1))
            {
                throw Logger.logException(new AccountNotFoundError("Аккаунт не найден"));
            }

            BankAccount account0 = BankAccount.accounts[accountId0];
            BankAccount account1 = BankAccount.accounts[accountId1];*/


            transfer(account0, account1, amount, type);
        }
    }
}
