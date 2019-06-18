
namespace Shop.Web.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ProductsController : Controller
    {
        /*Como eliminamos el IRepositoty clase, debemos cusar los demás repositorios. Además dar revisada a los métodos
         ya que algunos cambian, otros se vuelven asyncronos, entre otros*/
        //private readonly IRepository repository;

        private readonly IProductRepository productRepository;
        public IUserHelper userHelper;

        //eliminamos el DataContext e inyectamos la Interfaz Repositorio
        /*Como adicionamos el IUserHelper, lo vamos a inyectar en nuestro products controller e inizalizo prop userHelper*/
        public ProductsController(IProductRepository productRepository , IUserHelper userHelper) //antes era IRepository
        {
            this.productRepository = productRepository;
            this.userHelper = userHelper;
        }

        /*Index(acción por defecto del controlador) pinta la lista de productos, le estaba pasando aquí la lista 
        de productos obtenida de la DB ahora le va a pasar los prodictos del repositorio. Ya no es Asincrono*/
        // GET: Products
        public IActionResult Index()
        {
            /* la Vista pinta esos productos. (El controlador maneja la lógica, la vista lo que el usuario ve)
             y los modelos son los objetos que estamos trasportando entre V y C) */
           // Cambió el método 
           //return View(this.repository.GetProducts());
            return View(this.productRepository.GetAll());
        }

        // GET: Products/Details/5
        // Las acciones Index y details solo tienen GET ya que son solo de consulta (lectura)
        // Cambió el método y repository por productRepository
        //public IActionResult Details(int? id)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = this.repository.GetProduct(id.Value);
            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        // Cuendo le digo NEW el me pinta un formularion en blanco
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        //Cuando le digo SAVE se ejecuta el botón Submit del formulario, el cual ejectula la acción POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                /*Si el modelo es valido, entonces accedemos al producto por repository, adicionamos el 
                 producto que queremos guardar en BD AddProduct, lo guardo con SaveAllAsync() y si lo logra
                 grabar, pinta el producto =>RedirectToAction*/

                /*Después de iyectar "userHelper" adicionamos la captura del usruario que ha creado el producto en "product.User"
                 Ir a Edit también y hacer el mismo cambio para que el usuaro no quede nulo.*/
                //TODO: To change for the logged user
                product.User = await this.userHelper.GetUserByEmailAsync("jedgara@gmail.com");
                //cambio aquí
                //this.repository.AddProduct(product);
                //await this.repository.SaveAllAsync
                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            /*Si el modelo no es valido (no pasa las dataanotations), vuelve a llamar la acción create y le pasa
             lo que lleva. Cuando se envia el post con el product se está enviando un producto nullo, el usuario
             llena todos o parcialmente los campos de product, cuando vuelve al POST regresa con los campos que
             el usuario haya ingresado (ya no es null).*/
            return View(product);
        }

        // GET: Products/Edit/5
        //public IActionResult Edit(int? id)
        // Cambió aquí el metodo
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // busqueme el producto por el Value del id del producto
            //var product = this.repository.GetProduct(id.Value);
            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            //pinta el formulario con los datos con los datos previos para que sean modificados
            return View(product);
        }

        // POST: Products/Edit/5
        // realizamos los cambios que el usuario realiza en el edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //TODO: To change for the logged user
                    product.User = await this.userHelper.GetUserByEmailAsync("jedgara@gmail.com");
                    //Cambió acá
                    //this.repository.UpdateProduct(product);
                    //await this.repository.SaveAllAsync();// aquí es bueno validar si guardó o nó
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    //Cambió acá
                    //if (!this.repository.ProductExists(product.Id))
                    if (!await this.productRepository.ExistAsync(product.Id))
                        {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        // si no esxiste el producto retotna notfound, si existe lo busca
        //cambió también después de implementar Ipro/countryRepository
       //public IActionResult Delete(int? id)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var product = this.repository.GetProduct(id.Value);
            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            //si lo encuentra se va para la vista Delete
            return View(product);
        }

        // POST: Products/Delete/5
        // cuando hagan el submit se ejecuta la acción delete, el POST delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // cambiaron
            //var product = this.repository.GetProduct(id);
            //this.repository.RemoveProduct(product);
            //await this.repository.SaveAllAsync();
            var product = await this.productRepository.GetByIdAsync(id);
            await this.productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
