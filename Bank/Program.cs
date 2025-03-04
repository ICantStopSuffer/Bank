namespace Bank {
                                            
    public class Program {

        public class Logger {
            private static string fileName = "transaction_errors.log";

            public static void clear() {
                File.WriteAllText(fileName, string.Empty);
            }

            public static void log(String msg) {
                File.AppendAllText(fileName, msg + "\n");
            }

            public static Exception logException(Exception exception) {
                log(@$"Вемямя: {TimeOnly.FromDateTime(DateTime.Now)}
Тип: {exception.GetType().ToString().Split("+")[1]}
Описание: {exception.ToString().Split("+")[1].Split(":")[1]}");
                return exception;
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

        public class BankAccount {
            public static Dictionary<int, BankAccount> accounts = new Dictionary<int, BankAccount>();
            private static int nextId;
            public int id { private set; get; }
            public double balance { private set; get; }

            public BankAccount() {
                id = nextId++;
                accounts.Add(id, this);
            }

            public void withdraw(double amount) {
                if (amount > balance) {
                    throw Logger.logException(new InsufficientFundsError("Количество привышает баланс"));
                }
                if (amount < 0) {
                    throw Logger.logException(new InvalidAmountError("Количество должно быть положительным"));
                }

                balance -= amount;
            }

            public void deposit(double amount) {
                if (amount < 0) {
                    throw Logger.logException(new InvalidAmountError("Количество должно быть положительным"));
                }

                balance += amount;
            }

            public override string ToString() {
                return $"{id}: {balance}";
            }
        }

        public class TransactionManager {
            public TransactionManager() { }

            public void transfer(BankAccount account0, BankAccount account1, double amount) {
                if (account0 == null || account1 == null) {
                    throw Logger.logException(new AccountNotFoundError("Аккаунт не найден"));
                }

                try {
                    account0.withdraw(amount);
                    account1.deposit(amount);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }

            public void transfer(int accountId0, int accountId1, double amount) {
                if (!BankAccount.accounts.ContainsKey(accountId0) || !BankAccount.accounts.ContainsKey(accountId1)) {
                    throw Logger.logException(new AccountNotFoundError("Аккаунт не найден"));
                }

                BankAccount account0 = BankAccount.accounts[accountId0];
                BankAccount account1 = BankAccount.accounts[accountId1];

                transfer(account0, account1, amount);
            }
        }

        static void Main(string[] args) {
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