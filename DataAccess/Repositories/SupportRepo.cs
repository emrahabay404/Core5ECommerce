using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
   public class SupportRepo : GenericRepository<SupportRequest>
    {
        public Context C = new();
        public List<SupportRequest> GetListWith()
        {
            return C.SupportRequests.Include(x => x.AppUser).ToList();
        }

    }
}
