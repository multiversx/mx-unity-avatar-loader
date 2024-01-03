using NUnit.Framework;
using System;
using MultiversX.Avatar.Loader.Editor.WalletConnectMx;

namespace MultiversX.Avatar.Core.Tests
{
    public class MultiversxContextTests
    {
        [Test]
        public void TestGetShortAddress()
        {
            // Arrange
            WalletConnectFacade.Instance.Address = "0x1234567890123456789012345678901234567890";

            // Act
            string result = WalletConnectFacade.Instance.GetShortAddress();

            // Assert
            Assert.AreEqual("0x12345678...234567890", result);
        }

        [Test]
        public void TestGetShortAddress_WithShortAddress()
        {
            // Arrange
            WalletConnectFacade.Instance.Address = "0x12345";

            // Act
            string result = WalletConnectFacade.Instance.GetShortAddress();

            // Assert
            Assert.AreEqual("0x12345", result);
        }

        [Test]
        public void TestGetShortAddress_WithNullAddress()
        {
            // Arrange
            WalletConnectFacade.Instance.Address = null;

            // Act
            NullReferenceException ex = Assert.Throws<NullReferenceException>(
                () => WalletConnectFacade.Instance.GetShortAddress()
            );

            // Assert
            Assert.That(
                ex.Message,
                Is.EqualTo("Object reference not set to an instance of an object")
            );
        }
    }
}
