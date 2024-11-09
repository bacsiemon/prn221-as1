using DataAccess;
using DataAccess.Models;
using Repositories.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public class OrderRepository : IOrderRepository
	{
		private readonly string _fileName = "order.json";
		private List<Order> _orders;
		private IGenericJsonTool<Order> _tool;

		public OrderRepository()
		{
			if (_tool == null) _tool = new GenericJsonTool<Order>();

			if (!File.Exists(_fileName) || new FileInfo(_fileName).Length == 0)
			{
				SeedData();
			}
			else
			{
				_orders = _tool.Read(_fileName);
				if (_orders == null || _orders.Count == 0)
					SeedData();
			}
		}

		public Order? Get(int orderId)
		{
			return _orders.FirstOrDefault(o => o.OrderId == orderId);
		}

		public List<Order> GetAll()
		{
			_orders = _tool.Read(_fileName);
			return _orders;
		}

		public bool Add(Order order)
		{
			try
			{
				order.OrderId = GenerateId();
				_orders.Add(order);
				_tool.Write(_fileName, _orders);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public bool Update(Order order)
		{
			
			try
			{
				Order existing = _orders.FirstOrDefault(m => m.OrderId == order.OrderId);
				if (existing == null) return false;

				existing.MemberId = order.MemberId;
				existing.OrderDate = order.OrderDate;
				existing.RequiredDate = order.RequiredDate;
				existing.ShippedDate = order.ShippedDate;
				existing.Freight = order.Freight;

				File.Delete(_fileName);
				_tool.Write(_fileName, _orders);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public bool Delete(int orderId)
		{
			try
			{
				Order existing = _orders.FirstOrDefault(m => m.OrderId == orderId);
				if (existing == null) return false;
				_orders.Remove(existing);

				File.Delete(_fileName);
				_tool.Write(_fileName, _orders);
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
			foreach (Order order in _orders)
			{
				if (order.OrderId > max)
					max = order.OrderId;
			}
			return max + 1;
		}
		private void SeedData()
		{
			_orders = new List<Order>()
			{
				new Order()
				{
					OrderId = 1,
					MemberId = 1,
					OrderDate = DateTime.Now,
					RequiredDate = DateTime.Now.AddDays(3),
					ShippedDate = DateTime.Now.AddDays(2),
					Freight = 100
				},

				new Order()
				{
					OrderId = 2,
					MemberId = 1,
					OrderDate = DateTime.Now,
					RequiredDate = DateTime.Now.AddDays(4),
					ShippedDate = DateTime.Now.AddDays(1),
					Freight = 110
				},
				new Order()
				{
					OrderId = 3,
					MemberId = 1,
					OrderDate = DateTime.Now.AddDays(-3),
					RequiredDate = DateTime.Now.AddDays(2),
					ShippedDate = DateTime.Now.AddDays(0),
					Freight = 100
				}
			};
			_tool.Write(_fileName, _orders);
		}

	}
}
