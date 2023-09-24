using AmazonClone.Model;

namespace AmazonClone.Test.Model
{

    public class UserShould
    {
        [Fact]
        public void HaveUserName()
        {
            User sut = new User();
            sut.UserName = "username";
            Assert.Equal("username", sut.UserName);
        }

    }
}
