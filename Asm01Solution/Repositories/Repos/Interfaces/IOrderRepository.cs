using DataAccess.Models;

namespace Repositories.Repos.Interfaces
{
    public interface IOrderRepository
    {
        bool Add(Order order);
        bool Delete(int orderId);
        Order? Get(int orderId);
        List<Order> GetAll();
        bool Update(Order order);
    }
}