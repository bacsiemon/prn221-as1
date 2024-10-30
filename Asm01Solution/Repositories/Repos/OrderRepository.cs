using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
	public class OrderRepository
	{
		private Assignment1Context _context;

        public OrderRepository()
        {
            if (_context == null) _context = new();
        }

		public List<Order> GetAll() 
		{
			return _context.Orders.ToList();
		}
    }
}
