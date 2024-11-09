using DataAccess.Models;
using DataAccess;
using Repositories.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public class ProductRepository : IProductRepository
	{

		private readonly string _fileName = "product.json";
		private List<Product> _products;
		private IGenericJsonTool<Product> _tool;


		

		public ProductRepository()
		{
			if (_tool == null) _tool = new GenericJsonTool<Product>();

			if (!File.Exists(_fileName) || new FileInfo(_fileName).Length == 0)
			{
				SeedData();
			}
			else
			{
				_products = _tool.Read(_fileName);
				if (_products == null || _products.Count == 0)
					SeedData();
			}
		}

		public List<Product> GetAll()
		{
			_products = _tool.Read(_fileName);
			return _products;
		}

		public bool Add(Product product)
		{
			try
			{
				product.ProductId = GenerateId();
				_products.Add(product);
				_tool.Write(_fileName, _products);
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
				Product existing = _products.FirstOrDefault(p => p.ProductId == product.ProductId);
				if (existing == null) return false;

				_products.Remove(existing);
				File.Delete(_fileName);
				_tool.Write(_fileName, _products);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		

		public bool Update(Product pd) 
		{
			try
			{
				Product existing = _products.FirstOrDefault(p => p.ProductId == pd.ProductId);
				if (existing == null) return false;

				existing.CategoryId = pd.CategoryId;
				existing.ProductName = pd.ProductName;
				existing.Weight = pd.Weight;
				existing.UnitPrice = pd.UnitPrice;
				existing.UnitsInStock = pd.UnitsInStock;

				File.Delete(_fileName);
				_tool.Write(_fileName, _products);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
        }

		private int GenerateId()
		{
			int max = 0;
			foreach (Product product in _products)
			{
				if (product.ProductId > max)
					max = product.ProductId;
			}
			return max + 1;
		}

		private void SeedData()
		{
			_products = new()
			{
				new Product()
				{
					ProductId = 1,
					CategoryId = 1,
					ProductName = "Asus TUF F15 Pro",
					Weight = "2.3 KG",
					UnitPrice = 1500,
					UnitsInStock = 12
				},
				new Product()
				{
					ProductId = 2,
					CategoryId = 2,
					ProductName = "Acer Nitro 5",
					Weight = "1.8 KG",
					UnitPrice = 1000,
					UnitsInStock = 8
				},
				new Product()
				{
					ProductId = 3,
					CategoryId = 2,
					ProductName = "Acer Preadator Helios 300",
					Weight = "2.1 KG",
					UnitPrice = 2000,
					UnitsInStock = 10
				}
			};
			_tool.Write(_fileName, _products);
		}
	}
}
