using DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
   public class GenericRepository<T> where T : class, new()
   {
      public Context c = new();

      public List<T> TList()
      {
         return c.Set<T>().ToList();
      }

      public void TAdd(T p)
      {
         c.Set<T>().Add(p);
         c.SaveChanges();
      }

      public void TDelete(T t)
      {
         c.Set<T>().Remove(t);
         c.SaveChanges();
      }

      public void TUpdate(T p)
      {
         c.Set<T>().Update(p);
         c.SaveChanges();
      }

      public T TGet(int id)
      {
         return c.Set<T>().Find(id);
      }

      public List<T> TList(string p)
      {
         return c.Set<T>().Include(p).ToList();
      }

      public List<T> List(Expression<Func<T, bool>> filter)
      {
         return c.Set<T>().Where(filter).ToList();
      }

   }
}
