using NUnit.Framework;

namespace MultiversX.Avatar.Core.Tests
{
    public class AvatarLoaderContextTests
    {
        [Test]
        public void AvatarLoaderContextInitializesCorrectly()
        {
            // Arrange
            AvatarLoaderContext context = new AvatarLoaderContext();

            // Assert
            Assert.IsFalse(AvatarLoaderContext.IsLoaded);
            Assert.IsFalse(AvatarLoaderContext.IsFailed);
            Assert.IsNull(context.Avatar);
            Assert.IsNull(context.AvatarData);
        }
    }
}
