using Entities.Concrete;

namespace DataAccess.ViewModels
{
   public class RazorMultiModel
    {
        public IEnumerable<Product>  Products { get; set; }
        public IEnumerable<OrderDetail>  OrderDetails { get; set; }
        public IEnumerable<Order>  Orders { get; set; }

    }
}
