using BankAccountSystem;
using Newtonsoft.Json.Bson;

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
        public void ShouldCreateAccountNumberAndBalanceWhenConstructed()
        {
    
            //Assert
            string accountNumber = "123456";
            decimal initialBalance = 100.00m;

            //Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That(sut.AccountNumber, Is.EqualTo(accountNumber));
            Assert.That(sut.Balance, Is.EqualTo(initialBalance));

        }

        [TestCase("", 100, TestName ="ShouldThrowArgumentExceptionWhenConstructedWithEmptyAccountNumber")]
        [TestCase("123456", -100, TestName = "ShouldThrowArgumentExceptionWhenConstructedWithNegativeAmount")]
        [TestCase(null, 100, TestName = "ShouldThrowArgumentExceptionWhenConstructedWithNullAccountNumberAndValidAmount")]
        [TestCase("", -100, TestName = "ShouldThrowArgumentExceptionWhenConstructedWithEmptyAccountNumberAndNegativeBalance")]
        public void EmptyAccountNumberShouldThrowArgumentException(string? accountNumber, decimal initialBalance)
        {

            // Act & Assert
            try
            {
                BankAccount sut = new BankAccount(accountNumber, initialBalance);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail();
        }

        [Test]
        public void ShouldIncreaseBalanceWhenValidAmountDeposited()
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
        public void ShouldThrowArgumentExceptionWhenNegativeAmountDeposited()
        {

            //Assert
            string accountNumber = "123456";
            decimal initialBalance = 100.00m;

            //Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            var ex = Assert.Throws<ArgumentException>(() => sut.Deposit(-60m));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Deposit amount must be positive!"));

        }


        [Test]
        public void ShouldDecreaseBalanceWhenValidAmountIsWithrawed()
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
        public void ShouldThrowArgumentExceptionWhenNegativeAmountIsWithrawed()
        {
            //Assert
            string accountNumber = "123456";
            decimal initialBalance = 100.00m;

            //Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            var ex = Assert.Throws<ArgumentException>(() => sut.Withdraw(-60m));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Withdraw amount must be positive!"));
        }

        [Test]
        public void ShouldThrowInvalidOperationExceptionWhenAmountGreaterThanBalanceIsWithrawed()
        {
            //Assert
            string accountNumber = "123456";
            decimal initialBalance = 100.00m;

            //Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            var ex = Assert.Throws<InvalidOperationException>(() => sut.Withdraw(300m));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Insufficient balance!"));
        }


        [Test]

        public void ShouldReturnCurrentAccountStatusAsLowIfBalanceBelowHundred()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 99m;

            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            sut.GetAccountStatus();

            // Assert
            Assert.That(sut.GetAccountStatus(), Is.EqualTo("Low"));
        }

        [Test]
        public void ShouldReturnAccountStatusAsNormalIfBalanceGreaterThanOrEqualsHundred()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 200m;

            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That(sut.GetAccountStatus(), Is.EqualTo("Normal"));
        }

        [Test]
        public void ShouldReturnAccountStatusAsHighIfBalanceGreaterThanThousand()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 2000m;

            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That(sut.GetAccountStatus(), Is.EqualTo("High"));

        }


        [Test]
        public void ShouldTransferAmountWhenAmountAndRecipientAreValid()
        {
            // Arrange
            string accountNumber = "123456";
            decimal initialBalance = 100m;

            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

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

        // Boundary Value Analysis for Constructor

        [TestCase(0, TestName ="ConstructorWithZeroBalance")]
        [TestCase(0.01, TestName = "ConstructorWithSmallPositiveValue")]
        [TestCase(1000000000, TestName = "ConstructorWithVeryLargePositiveValue")]
        public void ShouldInitializeBankAccountWithBoundaryValuesAsInitialBalance(decimal initialBalance)
        {
            // Arrange
            string accountNumber = "123456";

            // Act
            BankAccount sut = new BankAccount(accountNumber, initialBalance);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(initialBalance));
            Assert.That(sut.AccountNumber, Is.EqualTo(accountNumber));
        }

        // Boundary value analysis for Deposit


        [TestCase(0.01, TestName = "DepositSmallAmount")]
        [TestCase(123456, TestName = "DepositLargeAmount")]
        [TestCase(2123456998, TestName = "DepositVeryLargeAmount")]
        public void ShouldIncreaseBalanceWhenBoundaryValueAmountsDeposited(decimal amount)
        {
            // Arrange
            string accountNumber = "123456";

            // Act
            BankAccount sut = new BankAccount(accountNumber, 0m);

            // Depositing a very small amount
                sut.Deposit(amount);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(amount));
        }

        [TestCase(-0.01, TestName = "DepositAmountJustBelowTheBoundary")]
        [TestCase(0, TestName = "DepositAmountZero")]
        public void ShouldThrowArgumentExceptionWhenDepositingInvalidAmount(decimal amount)
        {
            // Arrange
            string accountNumber = "123456";
            BankAccount sut = new BankAccount(accountNumber, 0);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => sut.Deposit(amount));
            Assert.That(ex.Message, Is.EqualTo("Deposit amount must be positive!"));
        }


        [TestCase(0.01, TestName = "WithdrawSmallAmount")]
        [TestCase(123456, TestName = "WithdrawLargeAmount")]
        [TestCase(2123456998, TestName = "WithdrawAVeryLargeAmount")]
        public void ShouldDecreaseBalanceWhenBoundaryValueAmountsAreWithdrawed(decimal amount)
        {
            // Arrange
            string accountNumber = "123456";

            // Act
            BankAccount sut = new BankAccount(accountNumber, amount);
            decimal initialBalance = sut.Balance;

            // Depositing a very small amount
            sut.Withdraw(amount);

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(initialBalance - amount));

        }

        [TestCase(-0.01, TestName = "WithdrawAmountJustBelowTheBoundary")]
        [TestCase(0, TestName = "WithdrawAmountZero")]
        public void ShouldThrowArgumentExceptionWhenWithdrawingInvalidAmount(decimal amount)
        {
            // Arrange
            string accountNumber = "123456";
            BankAccount sut = new BankAccount(accountNumber, 100m);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => sut.Withdraw(amount));
            Assert.That(ex.Message, Is.EqualTo("Withdraw amount must be positive!"));
        }


        [TestCase(99.99, ExpectedResult = "Low", TestName = "GetAccountStatusAsLowBelow100")]
        [TestCase(100, ExpectedResult = "Normal", TestName = "GetAccountStatusAsNormalAt100")]
        [TestCase(100.01, ExpectedResult = "Normal", TestName = "GetAccountStatusAsNormalAbove100")]
        [TestCase(999.99, ExpectedResult = "Normal", TestName = "GetAccountStatusAsNormalBelow1000")]
        [TestCase(1000, ExpectedResult = "High", TestName = "GetAccountStatusAsHighAt1000")]
        [TestCase(1000.01, ExpectedResult = "High", TestName = "GetAccountStatusAsHighAbove1000")]
        public string ShouldReturnCorrectStatusBasedOnBoundaryValues(decimal amount)
        {
            // Arrange
            string accountNumber = "123456";

            // Act
            BankAccount sut = new BankAccount(accountNumber, amount);
            string status = sut.GetAccountStatus();

            // Assert
            return status;
        }

        [Test]
        public void TransferToShouldTranserEntireAmount()
        {

            BankAccount sender = new BankAccount("123456", 100m);

            sender.TransferTo(_recipient, 100m);

            // Assert
            Assert.That(sender.Balance, Is.EqualTo(0m));
            Assert.That(_recipient.Balance, Is.EqualTo(120m));
        }

        [Test]
        public void TransferToShouldTranserAVerySmallAmount()
        {
           
            BankAccount sender = new BankAccount("123456", 100m);

            sender.TransferTo(_recipient, 0.01m);

            // Assert
            Assert.That(sender.Balance, Is.EqualTo(99.99m));
            Assert.That(_recipient.Balance, Is.EqualTo(20.01m));
        }

        [Test]
        public void TransferToShouldTranserToHighBalanceAccount()
        {

            BankAccount sender = new BankAccount("123456", 100m);
            _recipient = new BankAccount("654321", 1_000_000_000m);

            sender.TransferTo(_recipient, 50m);

            // Assert
            Assert.That(sender.Balance, Is.EqualTo(50m));
            Assert.That(_recipient.Balance, Is.EqualTo(1_000_000_050m));
        }

        // Combinatorial And Pairwise Testing

        [Test]
        [Combinatorial]
        public void ShouldUpdateBalanceAndStatusCorrectlyAfterDepositAndWithdrawal(
            [Values(1000.00, 1.00)] decimal initialBalance,
            [Values(10, 500, 1000)] decimal depositeAmount,
            [Values(5, 100, 1500)] decimal withdrawAmount)
        {
            // Arrange
            BankAccount sut = new BankAccount("123456", initialBalance);

            // Act
            sut.Deposit(depositeAmount);
            decimal expectedBalance = initialBalance + depositeAmount;

            try
            {
                sut.Withdraw(withdrawAmount);
                expectedBalance -= withdrawAmount;
            }
            catch (InvalidOperationException ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Insufficient balance!"));
            }

            string expectedStatus;

            if (expectedBalance < 100)
            {
                expectedStatus = "Low";
            }
            else if (expectedBalance < 1000)
            {
                expectedStatus = "Normal";
            }
            else
            {
                expectedStatus = "High";
            }

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(expectedBalance));
            Assert.That(sut.GetAccountStatus, Is.EqualTo(expectedStatus));

        }

        [Test]
        [Pairwise]
        public void ShouldUpdateBalanceAndStatusCorrectlyAfterDepositAndWithdrawPairwise(
            [Values(1000.00, 1.00)] decimal initialBalance,
            [Values(10, 500, 1000)] decimal depositeAmount,
            [Values(5, 100, 1500)] decimal withdrawAmount)
        {
            // Arrange
            BankAccount sut = new BankAccount("123456", initialBalance);

            // Act
            sut.Deposit(depositeAmount);
            decimal expectedBalance = initialBalance + depositeAmount;

            try
            {
                sut.Withdraw(withdrawAmount);
                expectedBalance -= withdrawAmount;
            }
            catch (InvalidOperationException ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Insufficient balance!"));
            }

            string expectedStatus;

            if (expectedBalance < 100)
            {
                expectedStatus = "Low";
            }
            else if (expectedBalance < 1000)
            {
                expectedStatus = "Normal";
            }
            else
            {
                expectedStatus = "High";
            }

            // Assert
            Assert.That(sut.Balance, Is.EqualTo(expectedBalance));
            Assert.That(sut.GetAccountStatus, Is.EqualTo(expectedStatus));

        }

    }
}