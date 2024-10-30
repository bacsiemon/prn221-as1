using DataAccess.Models;

namespace Repositories.Repos.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();

		bool Add(Product product);
		bool Update(Product pd);

		bool Delete(Product product);


	}
}