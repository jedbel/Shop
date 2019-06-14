
namespace Shop.Web.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    // Se creó a partir de la clase Repository, para que cuando inyectemos lo hagamos con una configuración específica de la
    //Interfáz, lo que me vaa permitir pruebas Unitarias.
    public interface IRepository
    {
        void AddProduct(Product product);

        Product GetProduct(int id);

        IEnumerable<Product> GetProducts();

        bool ProductExists(int id);

        void RemoveProduct(Product product);

        Task<bool> SaveAllAsync();

        void UpdateProduct(Product product);
    }
}