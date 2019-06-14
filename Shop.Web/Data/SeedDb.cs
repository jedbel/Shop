
namespace Shop.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;

    // Esta clase solo corre si no hay productos (si se eliminan los que hay actualmente por ejemplo)
    public class SeedDb
    {
        private readonly DataContext context;
        private Random random;

        /* JB Injects the data context defined on StarUp class.
        When DataContext is called, the inyection occurs with that configuration*/
        public SeedDb(DataContext context)
        {
            this.context = context;
            this.random = new Random();
        }

        //JB SeedAsync is a method tha seed my Database.
        //EnsureCreatedAsync() => If the database currently creating, It wait until the DB creation process ends.
        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            // IF there aren't ANY(at least one) register return false and add the listed products
            if (!this.context.Products.Any())
            {
                this.AddProduct("iPhone X");
                this.AddProduct("Magic Mouse");
                this.AddProduct("iWatch Series 4");
                await this.context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name)
        {
            // Here we are the products defined above on Method SeddAsync (iPhone, Mouse...)
            this.context.Products.Add(new Product
            {
                Name = name,
                //Put on proce a random number betwwen 0 and 10000
                Price = this.random.Next(1000),
                IsAvailabe = true,
                Stock = this.random.Next(100)
            });
            // Then of adding this Class (seedDb), THE NEXT SPET IT´S CHANGE THE PROGRAM CLASS
        }
    }


}

