using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
   public class OrderDetailRepo : GenericRepository<OrderDetail>
    {
        public Context C = new();

        public List<OrderDetail> GetListWith(int id)
        {
            return C.OrderDetails.Where(x => x.OrderDetailId == id).Include(x=>x.Address).Include(x => x.Order).Include(x => x.Product).Include(x => x.Cargo).Include(x => x.OrderProcess).ToList();
        }

        public List<OrderDetail> StoreOrderList(int id)
        {
            return C.OrderDetails.Where(x => x.Product.Id == id).Include(x => x.Product).Include(x => x.Address).Include(x => x.Cargo).Include(x => x.OrderProcess).ToList();
        }


    }
}
