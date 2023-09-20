using AmazonClone.Model;
using AmazonClone.Repository.Interface;
using System.Linq.Expressions;

namespace AmazonClone.Service
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>?> GetCategories()
        {
            return await _unitOfWork.CategoryRepository.GetAll();
        }
        public async Task<IEnumerable<Category>?> GetCategories(Expression<Func<Category, bool>> predicate)
        {
            return await _unitOfWork.CategoryRepository.GetAll(predicate);
        }
        public void AddCategory(string name)
        {
            Category category = new Category(name);
            _unitOfWork.CategoryRepository.Add(category);
            _unitOfWork.SaveChanges();
        }
        public async void EditCategory(string from, string to)
        {
            var category = await _unitOfWork.CategoryRepository.Get(a => a.Name == from);
            if (category == null)
                return;
            category.Name = to;
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.SaveChanges();
        }
        public async void DeleteCategory(string name)
        {
            var category = await _unitOfWork.CategoryRepository.Get(c => c.Name == name);
            if (category == null)
                return;
            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.SaveChanges();
        }
    }
}
