
namespace Shop.Web.Data
{
    using System.Linq;
    using Entities;
    using Microsoft.EntityFrameworkCore;

    /*ProductRepository hace una implementaión del generic repository con productos y el Iproductrepository*/
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext context;

        /*ProductRepository inyecta el DataContext y se lo pasa al controctor de la suplercalse o clase base*/
        public ProductRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        /*17 Ctr+. IproductRepository > Implement Interface. Luego Ctrl+. > context (DataContext context) arriba
         > create and inizializate field,ya que tenemos que hacer una consulta directa al datacontext y como arriba
         estamos inyectando del datacontext y a su vez lo pasamos a la clase base, nos toca hacer que el context sea
         disponible durante toda la ejecución. Vamos a editar el método abajo...*/
        public IQueryable GetAllWithUsers()
        {
            /*17 Retorneme la relación con la tabla Users (Include) incluyame de los products, el product.User
             Para que el método quede generico, no lo ordenaremos aquí, y como el método retorna un Iqueryable
             lo puedo ordenar en el otro lado. La diferencia de este metodo con GEtAll es que este metodo si me traerá
             la relación con la tabla Users. Vamos al controlador MVC -> Controllers > ProductsControlers > Index Metod*/
            return this.context.Products.Include(p => p.User);
        }
    }

}
