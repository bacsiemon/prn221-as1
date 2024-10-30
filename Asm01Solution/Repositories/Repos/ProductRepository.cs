using DataAccess.Models;
using Repositories.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public class ProductRepository : IProductRepository
	{
		private Assignment1Context _context;

		public ProductRepository()
		{
			if (_context == null) _context = new();
		}

		public bool Add(Product product)
		{
			try
			{
				_context.Products.Add(product);
				_context.SaveChanges();
				return true;
			}catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public bool Delete(Product product)
		{
			try
			{
				_context.Products.Remove(product);
				_context.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public List<Product> GetAll()
		{
			return _context.Products.ToList();
		}

		public bool Update(Product pd) 
		{
			try
			{
				Product existing = _context.Products.FirstOrDefault(p => p.ProductId == pd.ProductId);
				if (existing == null) return false;

				existing.CategoryId = pd.CategoryId;
				existing.ProductName = pd.ProductName;
				existing.Weight = pd.Weight;
				existing.UnitPrice = pd.UnitPrice;
				existing.UnitsInStock = pd.UnitsInStock;

				_context.Products.Update(existing);
				_context.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
        }
	}
}
