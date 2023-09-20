using AmazonClone.Model;

namespace AmazonClone.Test.Model
{
    public class CategoryShould
    {
        [Fact]
        public void HaveName()
        {
            Category category = new Category("name");
            Assert.Equal("name", category.Name);
        }
    }
}