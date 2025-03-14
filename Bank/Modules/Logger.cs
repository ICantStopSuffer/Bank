using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Modules
{
    public class Logger
    {
        public static bool Logging = false;
        private static string fileName = "transaction_errors.log";
        private static string transactionFile = "transactions.log";

        public static void clear()
        {
            File.WriteAllText(fileName, string.Empty);
        }

        public static void log(string msg)
        {
            if (!Logging)
            {
                return;
            }
            File.AppendAllText(fileName, msg + "\n");
        }

        public static void logTransaction(BankAccount account0, BankAccount account1, double amount, valueType type) {

            File.AppendAllText(transactionFile, $"{account0.id}, {account1.id}, {amount}, {type}");
        }

        public static string viewTransactions() {
            string view = "";

            string[] lines = File.ReadAllLines(transactionFile);

            foreach (string line in lines) {
                string[] splitted = line.Split(",");
                string bankId0 = splitted[0];
                string bankId1 = splitted[1];
                double amount = Double.Parse(splitted[2]);
                valueType type = (valueType)Int32.Parse(splitted[3]);

                view += $"from: {bankId0}\n to: {bankId1}\n amount: {amount}\n type: {Enum.GetName(typeof(valueType), type)}\n\n";
            }

            return view;
        }

        public static Exception logException(Exception exception)
        {
            if (!Logging)
            {
                return exception;
            }
            log(@$"Вемямя: {TimeOnly.FromDateTime(DateTime.Now)}
Тип: {exception.GetType().ToString()}
Описание: {exception.ToString()}");
            return exception;
        }
    }
}