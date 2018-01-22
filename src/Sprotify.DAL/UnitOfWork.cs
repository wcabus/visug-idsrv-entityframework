using System.Threading.Tasks;

namespace Sprotify.DAL
{
    public class UnitOfWork
    {
        private readonly SprotifyDbContext _context;

        public UnitOfWork(SprotifyDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}