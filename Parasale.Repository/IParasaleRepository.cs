using System.Threading.Tasks;

namespace Parasale.Repository
{
    public interface IParasaleRepository
    {
        Task Add(object entity);
        void Delete(object entity);
        Task<bool> Save();
        void Update(object entity);
        void DeleteAll(object entity);
        bool DataExist(string id);
        bool QucikCollectionExist();
    }
}