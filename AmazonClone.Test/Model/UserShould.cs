using AmazonClone.Model;

namespace AmazonClone.Test.Model
{

    public class UserShould
    {
        [Fact]
        public void HaveUserName()
        {
            User sut = new User("name");
            Assert.Equal("username", sut.UserName);
        }

    }
}
