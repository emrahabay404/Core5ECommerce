using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
   public class WalletRepo : GenericRepository<Wallet>
    {

        public Context C = new();
        public List<Wallet> GetListWith()
        {
            return C.Wallets.Include(x => x.AppUser).ToList();
        }



        public List<Wallet> TGetImg(int id)
        {
            return C.Wallets.Where(x => x.WalletID == id).Include(x => x.AppUser).ToList();
        }





    }
}
