using DataAccess.Concrete;
using Entities.Concrete;

namespace DataAccess.ViewModels
{
    public class UsersModel
    {
        public Context C = new();
        public List<AppUser> Users1 { get; set; }

        public List<AppUser> FindAll()
        {
            //return C.Users1.ToList();
            return C.Users.ToList();
        }

        public AppUser Find(int id)
        {
            List<AppUser> users = FindAll();
            var ara = users.Where(a => a.Id == id).FirstOrDefault();
            return ara;
        }

    }
}
