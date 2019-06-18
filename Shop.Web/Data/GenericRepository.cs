
namespace Shop.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;

    /*GenericRepository va a ser una implementación generica de T, que va aimplementar la INterfazGenerica T, donde T es una clase*/
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DataContext context;

        /*en el construtor se inyecta el DataContext, como se hace en el repositorio que tenemos, Repository*/
        public GenericRepository(DataContext context)
        {
            this.context = context;
        }

        /*Set<T> estaclece la lista que se va a devolver, ya que no se sabe si es products, customers..., 
         AsNoTracking() es para que funcione el mético genérico*/
        public IQueryable<T> GetAll()
        {
            return this.context.Set<T>().AsNoTracking();
        }

        /*Me trae un ej prod/cust/count. Traigamelo del context, setee el modelo (Set<T>) y busquemelo de tal manera que
         el código se igual al código que le pasaron como parametro (e => e.Id == id)*/
        public async Task<T> GetByIdAsync(int id)
        {
            return await this.context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /*En el contexto setee T y adicionolo Asincrono ese modelo (en Repository lo gravaba inmetiatamente, no async)*/
        public async Task CreateAsync(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }

        /*Lo actualizo y quí si de inmediato lo grabo*/
        public async Task UpdateAsync(T entity)
        {
            this.context.Set<T>().Update(entity);
            await SaveAllAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            this.context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await this.context.Set<T>().AnyAsync(e => e.Id == id);

        }

        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }
    }

}
