
namespace Shop.Web.Data
{
    using Entities;
    using System.Linq;

    /*La interfaz de Productos (IProductRepositor), va a ser una implementación de la interfaz genérica IGenericRepository,
     * pero con productos*/
    public interface IProductRepository : IGenericRepository<Product>
    {
        /*17 Impelemento estem método para que me traiga todos los usuarios, pero en postman veo que los User=Null
         y eso no me sirve, yo quiero desde que carguen los usuarios en eltelefono, saber de que usuario pertenecen, pero no tiene Null porque en la DB esté nullo
         sino cuando armamos la expresión linq que me tragera los productos a esas tablas que están relacionadas, hay que decirles exlicitamente
         que me traiga esa relación. Asi que hay que imlementar el método. 
         Vamos a ProductRepository > IproductRepository (saca error) > ctrl+. > implementar la interfaz*/
        IQueryable GetAllWithUsers();
    }

}
