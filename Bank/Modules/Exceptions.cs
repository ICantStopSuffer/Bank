namespace Bank.Modules {
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
}