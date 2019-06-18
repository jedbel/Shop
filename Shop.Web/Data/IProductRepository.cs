
namespace Shop.Web.Data
{
    using Entities;

    /*La interfaz de Productos (IProductRepositor), va a ser una implementación de la interfaz genérica IGenericRepository,
     * pero con productos*/
    public interface IProductRepository : IGenericRepository<Product>
    {
    }

}
