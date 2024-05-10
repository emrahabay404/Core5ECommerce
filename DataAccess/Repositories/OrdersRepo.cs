using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
   public class OrdersRepo : GenericRepository<Order>
    {

        public Context C = new();
        public List<Order> GetListWith(int id)
        {
            return C.Orders.OrderByDescending(x => x.OrderDate).Where(x => x.Id == id).ToList();
        }


        public OrderDetail GetOrderId(int id)
        {
            return c.OrderDetails.Where(x => x.OrderId == id).Include(x => x.Product).FirstOrDefault();
        }


        public List<OrderDetail> GetListDetails(int orderid)
        {
            return C.OrderDetails.Where(z => z.OrderId == orderid).Include(x => x.Product).Include(y => y.OrderProcess).ToList();
        }



     

     


    }
}
