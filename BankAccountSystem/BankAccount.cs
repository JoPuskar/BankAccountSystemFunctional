namespace BankAccountSystem
{
    public class BankAccount
    {
        public string AccountNumber { get; }
        public decimal Balance { get; private set; }

        public BankAccount(string accountNumber, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new ArgumentException("Account number cannot be empty or null!");
            }
            if (initialBalance < 0  )
            {
                throw new ArgumentException("Initial balance cannot be negative!");
            }
            AccountNumber = accountNumber;
            Balance = initialBalance;

        }

        public void Deposit(decimal amount)
        { 
            if (amount <= 0)
                {
                    throw new ArgumentException("Deposit amount must be positive!");
                }
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            { 
                throw new ArgumentException("Withdraw amount must be positive!");
            }

            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient balance!");
            }

            Balance -= amount;
        }

        public string GetAccountStatus()
        {
            if (Balance < 100)
            {
                return "Low";
            }

            else if (Balance < 1000)
            {
                return "Normal";
            }

            else
            {
                return "High";
            }
        }

        public void TransferTo(BankAccount recipient, decimal amount)
        { 
            if (recipient == null)
            {
                throw new ArgumentNullException(nameof(recipient), "Recipient account cannot be null!");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be positive!");
            }

            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds for transfer!");
            }

            Withdraw(amount);
            recipient.Deposit(amount);
        }

    }
}
