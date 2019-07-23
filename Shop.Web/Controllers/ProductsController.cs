
namespace Shop.Web.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Models;

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
        /*15 - cambiamos lo que devuelve el create, ya que es un productviewmodel*/
        //public async Task<IActionResult> Create(Product product)
        public async Task<IActionResult> Create(ProductViewModel view)

        {
            if (ModelState.IsValid)
            {
                /*Si el modelo es valido, entonces accedemos al producto por repository, adicionamos el 
                 producto que queremos guardar en BD AddProduct, lo guardo con SaveAllAsync() y si lo logra
                 grabar, pinta el producto =>RedirectToAction*/

                /*Después de iyectar "userHelper" adicionamos la captura del usruario que ha creado el producto en "product.User"
                 Ir a Edit también y hacer el mismo cambio para que el usuaro no quede nulo.*/

                /*15 - vamos a ver si el usuario seleccionó foto, pues no es obligatorio. En path estara la ruta donde
                 * vanos a guardar la foto. Cuando subamos la foto al servidor quedará al www.root, dentro en images vamos 
                 * crear una subcarpeta llamada produsts para introducir todas las imagenes de products.
                 */
                var path = string.Empty;

                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    /*15-Toma la imagen y la sube a "wwwroot\\images\\Products", y lo concatena con el nombre del archivo
                     FileName. En path queda la ruta donde se guardará enel servidor, queda subirla, esto creando un stream
                     con el path y que nos cree un archivo (FileMode.Create), ya el copytoasync toma el contenido del
                     archivo y me lo graba en el stream (copia carpeta local al servidor)*/
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(), 
                        "wwwroot\\images\\Products",
                        view.ImageFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    /*15 - aquí la ruta que vamos a guardar en l abase de datos. Vamos a interpolar (~ ruta relativa
                     * segun el ambiente donde esté trabajando) y lo concatenamos con el nombre del archivo. Ahora tenemos
                     * un problema, que a la base de datos se envía es entidades PRODUCTS (return View(product)130) y yo
                     * tengo es productviewmodel. Toca transformarlo, creamos var product  y método ToProduct*/
                    path = $"~/images/Products/{view.ImageFile.FileName}";
                }

                var product = this.ToProduct(view, path);
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
            /*15- cambiaos ya que no es product sino vista*/
            //return View(product);
            return View(view);
        }

        /* 15 - ToProduct-> le paso un ProductViewModel y nos devuelve un objeto de la clase Product, e igualamos
         atributo a atributo para hacer la transformación del obejto. Luego de la transf lo mandamos a DB
         Ya aquí está listo para subir imagenes, pero para probar necesitamos verlas, y esolo ahcemos en INDEX
         (vista Index de Products)*/
        private Product ToProduct(ProductViewModel view, string path)
            {
            return new Product
            {
                Id = view.Id,
                ImageUrl = path,
                IsAvailabe = view.IsAvailabe,
                LastPurchase = view.LastPurchase,
                LastSale = view.LastSale,
                Name = view.Name,
                Price = view.Price,
                Stock = view.Stock,
                User = view.User
            };

        }

        // GET: Products/Edit/5
        //public IActionResult Edit(int? id)
        // Cambió aquí el metodo
        /*16-210719- EDIT IMAGES. La diferencia aquí es que si le digo Edit, le estoy pasando un product. Ahora aquí
         * tenemos que convertir el product a productviewmodel porque le tenemos que pasar a la vista un productviewmodel
         (lo contrario tutorial 15). Es decir, para ello iniciamos creando la variable "view"*/
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

            /*16- para convertir el product a productviewmodel. Cambiamos "return View(product)", le pasamos un view
             * "ToProductViewModel" es el método para transformar un obejto en otro*/
            var view = this.ToProductViewModel(product);
            //pinta el formulario con los datos con los datos previos para que sean modificados
            return View(view);
        }

        /*16- Definimos el método aquí. Para convertir, pasamos cada atributo de unlado para otro (copiamos del método Toproduct,
         * ya lo hicimos), eliminar el path y adicionar el ImageUrl. Le estamos mosrando la foto actual para que el usuario
         decida si quiere cambiarla.Después vamos a la vista y cambiamos allí lo que recibe, ya recibe es un productViewModel
         es decir cambiamos Shop.Web.Data.Entities.Product por Shop.Web.Models.ProductViewModels*/
         
        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }

        // POST: Products/Edit/5
        // realizamos los cambios que el usuario realiza en el edit
        /*16_2 - Después de estar en el EdiTView, volvemos acá. Ya no vamos a recibir un objecto de la clase product
          vamos a recibirlo del productviewmodel, cambiamos Edit(int id, Product product)*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel view)
        {
            //if (id != Product.Id) cambié
            if (id != view.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*16 Hacer algo similar al create, preguntar si hubo foto, si el campo imageFile viene con un valor es debido a que
                     el usuario selccionó una foto que quiere subir. Aquí adiciono lo mismo del create*/
                     /*16 ya no inicializmaos path en empty, ahora lo inicializamos con el IMAGEurl (imagen original), para esto le hicimos
                      el HIDDEN*/
                    var path = view.ImageUrl;

                    if (view.ImageFile != null && view.ImageFile.Length > 0)
                    {
                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Products",
                            view.ImageFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{view.ImageFile.FileName}";
                    }

                    var product = this.ToProduct(view, path);
                    /*Hasta Aquí copié del create*/

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
                    /*16 ya no es ExistAsync(product.Id), sino View id*/
                    if (!await this.productRepository.ExistAsync(view.Id))
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
            //return View(product);
            return View(view);
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
