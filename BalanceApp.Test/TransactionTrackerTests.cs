using BalanceApp.App;
namespace BalanceApp.Test
{
    public class TransactionTrackerTests
    {
        [Fact]
        public void NetBalanceForNone()
        {
            // Arrange
            TransactionTracker tracker = new();

            // Assert
            double expected = 0;
            double actual = tracker.NetBalance;
            Assert.Equal(expected, actual, 2);
        }

        [Fact]
        public void NetBalanceForBalancesSingle()
        {
            // Arrange
            TransactionTracker tracker = new();
            DatedAmount amount = new()
            {
                Amount = 100.25,
                AmountName = "Amazon Balance",
                Category = "Balance"
            };

            // Act
            tracker.Add(amount);

            // Assert
            double expected = 100.25;
            double actual = tracker.NetBalance;
            Assert.Equal(expected, actual, 2);
        }

        [Fact]
        public void NetBalanceForMultiple()
        {
            // Arrange
            TransactionTracker tracker = new();

            DatedAmount amount1 = new()
            {
                Amount = 100.50,
                AmountName = "Amazon Balance",
                Category = "Balance"
            };
            DatedAmount amount2 = new()
            {
                Amount = 0.75,
                AmountName = "Chase Balance",
                Category = "Balance"
            };

            // Act
            tracker.Add(amount1);
            tracker.Add(amount2);

            // Assert
            double expected = 101.25;
            double actual = tracker.NetBalance;
            
            Assert.Equal(expected, actual, 2);
        }

        [Fact]
        public void ThrowsOnInvalidCategory()
        {
            // Arrange
            TransactionTracker tracker = new();
            DatedAmount amount = new()
            {
                Category = "Not Allowed",
                Amount = 100,
                AmountName = "Happy Time"
            };

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => tracker.Add(amount));
        }
    }
}