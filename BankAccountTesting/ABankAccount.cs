using BankAccountSystem;

namespace BankAccountTesting
{
    public class BankAccountTesting
    {
        private BankAccount _recipient;

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


        }

        [Test]
        public void ShouldNotTransferWhenBalanceIsInsufficient()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 100m;


            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            _recipient = new BankAccount("654321", 20m);
            var ex = Assert.Throws<InvalidOperationException>(() => sut.TransferTo(_recipient, 300m));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Insufficient funds for transfer!"));
            Assert.That(sut.Balance, Is.EqualTo(100m));
            Assert.That(_recipient.Balance, Is.EqualTo(20m));

        }

        //[Test]
        //public void ShouldNotTransferWhenAmountIsInvalid()
        //{
        //    // Arrange
        //    string accountNumber = "123456";
        //    decimal initialBalance = 100m;


        //    // Act
        //    BankAccount sut = new BankAccount(accountNumber, initialBalance);



        //    // Assert
        //}
    }
}