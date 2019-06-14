
namespace Shop.Web.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    /* Se crea esta clase dedibo a que se modificó el controlador de Products para que no acceda a BD directamente, 
     si no que en su lugar acceda a esta clase, y Repository accederá a la DB, es decir, estamos colocando una capa intermedia
     entre el controlador y la DB (Repository), y así poder cambiar a otras conexiones de base de datos (engañar al controlados)
     Útil en pruebas unitarias (en donde se cambia por conexion a DB falsa, Mokeada)
     */
     // Vamos a acceder a este patrón mediante la interfáz (relamente así es el patrón repository) 
    public class Repository : IRepository
    {
        private readonly DataContext context;

        // Cuando esta clase se llama, ella ya sabe que debe usar la conexión definida en el starUp.
        public Repository(DataContext context)
        {
            this.context = context;
        }

        /* Vamos aquí a adiconar todos los métodos, que me cree/elimine, modifique un producto*/

        // Este método nos retorna un IEnumebabe de products (lista no instanciada, se puede convertir en list, genric colection..)
        // Retorna => cntext=conexion DB, Products.OrberBy = productos ordenados por Name usando Linq (p => p.Name)= expresion lamda 
        public IEnumerable<Product> GetProducts()
        {
            return this.context.Products.OrderBy(p => p.Name);
        }

        //Nos devuelve un pbjeto de la clase Products, le paso el id y el método lo busca/retorna, Find siempre busca por id
        public Product GetProduct(int id)
        {
            return this.context.Products.Find(id);
        }

        // Adiciona el producto "product" a la lista de productos
        public void AddProduct(Product product)
        {
            this.context.Products.Add(product);
        }

        //Actualiza el producto. A la colección Products le actualiza dicho product
        public void UpdateProduct(Product product)
        {
            this.context.Products.Update(product);
        }

        public void RemoveProduct(Product product)
        {
            this.context.Products.Remove(product);
        }

        /*Los metodos anteriores, update/remove/add dejan la transacción pendiente, hasta que no ejecutemos el método
         SaveChangesAsync (todos los metodos asincronos deben terminar en Async, por eso mi método se llama SaveAllAsync)
         Return await => espera que los cambios pendientes se graven. SaveCHangesAsync()=> devuelve la camtidad de registros
         modificados, así que si es mayor que 0 (al menos un cambio) devuelve TRUE*/
        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        /* Similar al método GetProducts, pero este método me devuelve TRUE si el producto existe
        (p => p.Id == id) => si hay un Id de producto(p.Id) igual al (id) ingresado*/
        public bool ProductExists(int id)
        {
            return this.context.Products.Any(p => p.Id == id);
        }

    }
}
