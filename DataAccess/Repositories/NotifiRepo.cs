using DataAccess.Concrete;
using Entities.Concrete;

namespace DataAccess.Repositories
{
   public class NotifiRepo : GenericRepository<Notification>
    {
        public Context C = new();
        public Notification nts = new();
        //public List<Product> GetWithStore(int id)
        //{
        //    return C.Products.Where(x => x.Id == id).ToList();
                //}





        public void NotifiAdd(int userid,int nottypeid)
        {
            nts.Id = userid;
            nts.NotifiTypeID = nottypeid;
            nts.SeenStatus = false;
            nts.NotifiDate = Convert.ToDateTime(DateTime.Now.ToLongTimeString());
            nts.Id = userid;
            C.Notifications.Add(nts); c.SaveChanges();
        }



    }
}
