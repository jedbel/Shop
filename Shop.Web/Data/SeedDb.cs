
namespace Shop.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;

    // Esta clase solo corre si no hay productos (si se eliminan los que hay actualmente por ejemplo)
    public class SeedDb
    {
        private readonly DataContext context;
        private readonly UserManager<User> userManager;
        private Random random;

        /* JB Injects the data context defined on StarUp class.
        When DataContext is called, the inyection occurs with that configuration*/

        /*Luego de hacer la migració, inyectamos otra clase llamada UserManager que se manipula con mi clase User
        No se configura en el StarUp ya que ya viene embebida por defaul en el .Net Core. Hay que inicializar el 
        campo userManajer con "Ctrl+." A continuación creo usuario (antes de productos) */
        public SeedDb(DataContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.random = new Random();
        }

        //JB SeedAsync is a method tha seed my Database.
        //EnsureCreatedAsync() => If the database currently creating, It wait until the DB creation process ends.
        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            // antes de crear el usuario, verifico que no exista con FindByEmailAsync
            var user = await this.userManager.FindByEmailAsync("jedgara@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Jedbel",
                    LastName = "Garcia",
                    Email = "jedgara@gmail.com",
                    UserName = "jedgara",
                    PhoneNumber = "3182149085"
                };

                // me crea el usuario con el password deseado, si el IdentityResult es difer a Success inta un error
                var result = await this.userManager.CreateAsync(user, "1234567");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            // IF there aren't ANY(at least one) register return false and add the listed products

            if (!this.context.Products.Any())
            {
                // Adiciono user ya que cambié la entrada del método abajo, i.e, estos productos los creo ese User
                this.AddProduct("iPhone X", user);
                this.AddProduct("Magic Mouse", user);
                this.AddProduct("iWatch Series 4", user);
                await this.context.SaveChangesAsync();
            }
        }

        //Como adicioné un Usuario, a la clase AddProduct le agrego un usuario (User user). Finalmente vamos al
        //StartUp configuramos las restricciones para mi password
        private void AddProduct(string name, User user)
        {
            // Here we are the products defined above on Method SeddAsync (iPhone, Mouse...)
            this.context.Products.Add(new Product
            {
                Name = name,
                //Put on proce a random number betwwen 0 and 10000
                Price = this.random.Next(1000),
                IsAvailabe = true,
                Stock = this.random.Next(100),
                User = user
            });
            // Then of adding this Class (seedDb), THE NEXT SPET IT´S CHANGE THE PROGRAM CLASS
        }
    }


}

