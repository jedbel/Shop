
namespace Shop.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    /*IGenericRepository<T> (notación diamante) es un Repositorio en el que se cambia T por cualquier clase, products, 
     customers, countries, etc*/
    public interface IGenericRepository<T> where T : class
    {
        /*Devuelve una lista de T*/
        IQueryable<T> GetAll();

        /*Pasamos un id, me devuelve un T (lo que yo le indico)*/
        Task<T> GetByIdAsync(int id);

        /*se le manda un T, y el método adiciona un T*/
        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);
    }

}
