
namespace Shop.Web.Data
{
    using Entities;

    /*ProductRepository hace una implementaión del generic repository con productos y el Iproductrepository*/
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        /*ProductRepository inyecta el DataContext y se lopasa al controctor de la suplercalse o clase base*/  
        public ProductRepository(DataContext context) : base(context)
        {
        }
    }

}
