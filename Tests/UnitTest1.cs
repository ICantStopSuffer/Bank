using Bank;
using Bank.Modules;

namespace Tests
{
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

            try {
                manager.transfer(0, 1, 100);
            } catch (Exception ex) {
                Assert.Fail(ex.Message);
            }
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

    [TestClass]
    public class Database {

        [TestMethod]
        public void Transfer() {
            TransactionManager manager = new TransactionManager();
            BankAccount bank = Program.context.accounts.ToList()[0];
            BankAccount bank2 = Program.context.accounts.ToList()[1];

            try {
                manager.transfer(bank, bank2, 1);
            }
            catch (Exception ex) {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void NoMoney() {
            TransactionManager manager = new TransactionManager();
            BankAccount bank = Program.context.accounts.ToList()[0];
            BankAccount bank2 = Program.context.accounts.ToList()[1];

            Assert.ThrowsException<InsufficientFundsError>(() => manager.transfer(bank, bank2, 50000));
        }

        [TestMethod]
        public void AccountNotFound() {
            TransactionManager manager = new TransactionManager();

            Assert.ThrowsException<AccountNotFoundError>(() => manager.transfer(0, 101, 1));
        }
    }

    [TestClass]
    public class ConvertValue {

        [TestMethod]
        public void Convert() {
            ValueConvertator convertator = new ValueConvertator();

            int value = 100;
            double usdValue = convertator.getKot()["USD"];

            Assert.IsTrue(convertator.Convert(value, valueType.USD) == value * usdValue);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAmountError))]
        public void Negative() {
            ValueConvertator convertator = new ValueConvertator();

            int value = -100;

            convertator.Convert(value, valueType.USD);
        }

        [TestMethod]
        public void Zero() {
            ValueConvertator convertator = new ValueConvertator();

            int value = 0;

            Assert.IsTrue(convertator.Convert(value, valueType.USD) == 0);
        }
    }
}