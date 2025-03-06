using Bank;

namespace Tests {
    [TestClass]
    public class AccountTest {
        [TestMethod]
        public void AccountWithdraw() {
            BankAccount bank = new BankAccount();
            bank.deposit(200);
            bank.withdraw(200);

            Assert.IsTrue(bank.balance == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientFundsError))]
        public void AccountWithdrawBIG() {
            BankAccount bank = new BankAccount();
            bank.deposit(200);
            bank.withdraw(400);

        }

        [TestMethod]
        public void AccountDeposit() {
            BankAccount bank = new BankAccount();
            bank.deposit(200);

            Assert.IsTrue(bank.balance == 200);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAmountError))]
        public void AccountDepositNegative() {
            BankAccount bank = new BankAccount();
            bank.deposit(-200);
        }
    }

    [TestClass]
    public class Transaction {
        [TestMethod]
        public void SuccsessfulTranfer() {
            TransactionManager manager = new TransactionManager();
            BankAccount bank = new BankAccount();
            bank.deposit(200);
            BankAccount bank2 = new BankAccount();
            bank.deposit(200);

            manager.transfer(0, 1, 100);
        }

        [TestMethod]
        public void AccountWithdrawBIG() {
            TransactionManager manager = new TransactionManager();
            BankAccount bank = new BankAccount();
            bank.deposit(200);
            BankAccount bank2 = new BankAccount();
            bank.deposit(200);


            Assert.ThrowsException<InsufficientFundsError>(() => manager.transfer(0, 1, 500));
            
        }

        [TestMethod]
        public void AccountDepositNegative() {
            TransactionManager manager = new TransactionManager();
            BankAccount bank = new BankAccount();
            bank.deposit(200);
            BankAccount bank2 = new BankAccount();
            bank.deposit(200);

            Assert.ThrowsException<InvalidAmountError>(() => manager.transfer(0, 1, -500));
        }

        [TestMethod]
        public void AccountDeposit() {
            TransactionManager manager = new TransactionManager();
            BankAccount bank = new BankAccount();
            bank.deposit(200);

            Assert.ThrowsException<AccountNotFoundError>(() => manager.transfer(0, 1, 100));
        }
    }

    [TestClass]
    public class Logging {
        [TestMethod]
        public void LogError() {
            Logger.Logging = true;

            Logger.logException(new ArgumentException());
        }
    }
}