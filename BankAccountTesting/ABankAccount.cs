using BankAccountSystem;

namespace BankAccountTesting
{
    public class BankAccountTesting
    {
        private BankAccount _recipient;

        [SetUp]
        public void Setup()
        { 
            _recipient = new BankAccount("654321", 20m); // Recipient account with initial balance
        }

        [Test]
        public void ShouldCreateBankAccountWithInitialBalance()
        {
    
            //Assert
            string accountNumber = "123456";
            decimal initialBalance = 100.00m;

            //Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.IsNotNull(sut);
            Assert.That(sut.AccountNumber, Is.EqualTo(accountNumber));
            Assert.That(sut.Balance, Is.EqualTo(initialBalance));

        }

        [Test]
        public void ShouldIncreaseBalanceWhenDeposited()
        {

            //Assert
            string accountNumber = "123456";
            decimal initialBalance = 100.00m;

            //Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            sut.Deposit(60m);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(160m));

        }

        [Test]
        public void ShouldDecreaseBalanceWhenWithrawed()
        {
            //Assert
            string accountNumber = "123456";
            decimal initialBalance = 100.00m;

            //Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            sut.Withdraw(60m);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(40m));
        }

        [Test]

        public void ShouldReturnCurrentAccountStatus()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 99m;

            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            sut.GetAccountStatus();

            // Assert
            Assert.That("Low", Is.EqualTo(sut.GetAccountStatus()));

            // Test for Normal status
            initialBalance = 100m;

            // Act
            sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That("Normal", Is.EqualTo(sut.GetAccountStatus()));

            // Test for High status
            initialBalance = 1000m;

            // Act
            sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That("High", Is.EqualTo(sut.GetAccountStatus()));


        }

        [Test]
        public void ShouldDecreaseBalanceWhenTransferredToValidRecipient()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 100m;


            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            _recipient = new BankAccount("654321", 20m);
            sut.TransferTo(_recipient, 60m);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(40m));
            Assert.That(_recipient.Balance, Is.EqualTo(80m));


        }

        [Test]
        public void ShouldNotTransferWhenBalanceIsInsufficient()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 100m;


            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);
            var ex = Assert.Throws<InvalidOperationException>(() => sut.TransferTo(_recipient, 300m));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Insufficient funds for transfer!"));
            Assert.That(sut.Balance, Is.EqualTo(100m));

        }

        [Test]
        public void ShouldNotTransferWhenAmountIsInvalid()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 100m;


            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);
            var ex = Assert.Throws<ArgumentException>(() => sut.TransferTo(_recipient, 0m));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Transfer amount must be positive!"));
            Assert.That(sut.Balance, Is.EqualTo(100m));
        }

        [Test]
        public void ShouldNotTransferWhenRecipientIsNull()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initiBalance = 100m;

            // Act
            BankAccount sut = new BankAccount(accountNumber, initiBalance);

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.TransferTo(null, 50m));

            Assert.Multiple(() =>
            {
                Assert.That(ex.ParamName, Is.EqualTo("recipient"));
                Assert.That(ex.Message, Does.Contain("Recipient account cannot be null!"));
                Assert.That(sut.Balance, Is.EqualTo(100m));
            });

        }

        // Boundary Value Analysis

        [Test]
        public void ShouldInitializeBankAccountWithBoundaryValuesAsInitialBalance()
        {
            // Passing 0
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 0m;
            
            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(0m));

            // Passing a very small positive value
            // Arrange 
            initialBalance = 0.00000005m;

            // Act
            sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(0.00000005m));

            // Passing a very large positive value
            // Assert 
            initialBalance = decimal.MaxValue;

            // Act
            sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(decimal.MaxValue));

        }

        [Test]
        public void ShouldIncreaseBalanceWhenBoundaryValueAmountsAreDeposited()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 0m;

            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);
            // Depositing a very small amount
            sut.Deposit(0.001m);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(0.001m));

            // Depositing a very large positive value

            sut = new BankAccount(accountNumber, initialBalance);

            sut.Deposit(5000000000900000m);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(5000000000900000m));

        }

        [Test]
        public void ShouldDecreaseBalanceWhenBoundaryValueAmountsAreWithdrawed()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 100m;

            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);
            // Depositing a very small amount
            sut.Withdraw(0.001m);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(99.999m));

            // Depositing a very large positive value

            sut = new BankAccount(accountNumber, 5000000000900000m);

            sut.Withdraw(5000000000900000m);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(0m));

        }

    }
}