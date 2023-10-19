using Parasale.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class ParasaleRepository : IParasaleRepository
    {
        private ParasaleDbContext _context;

        public ParasaleRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public virtual async Task Add(object entity)
        {
            await _context.AddAsync(entity);
        }

        public virtual void Update(object entity)
        {
            _context.Update(entity);
        }

        public virtual void Delete(object entity)
        {
            _context.Remove(entity);
        }

        public virtual async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool DataExist(string id)
        {
            return _context.Collections.Any(x=>x.appUser.Id == id);

        }
        public bool QucikCollectionExist()
        {
            return _context.qCollections.Any(x => x.QuickStart == true);

        }
        public virtual void DeleteAll(object entity)
        {
            try
            {
                _context.RemoveRange(entity);
            }
            catch (System.Exception ex)
            {

                throw  ;
            }
           
        }
    }
}