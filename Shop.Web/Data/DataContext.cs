
namespace Shop.Web.Data
{
    using Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /*Ahora nuestro DataContes va a heredar del Contexto IdentityDbContex  y no del DbContext tradicinal 
     (al que no le importan las tablas de usuario), este ya incluye las tablas de usario que maneja la seguridad 
     integrada del .NET Core y lo personalizamos al usuario que definimos <user>. Ahora también debemos
     configurar la relación entre la tabla product y la tabla User, esto definiendo la propiedad User en Product.cs
     (se define el objeto en el lado varios de la relación). Como adiconé esta relación debo hacer una nueva migración,
     pero primero debo borrar la base de datos, pues ya tengo 3 productos matriculados y no va a encontrar usuario que asignarle
     Ejecutamos "database drop"*/
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }

        /*Como adicionamos las clases RepositoryCountry y IRepCountry, debemos adicionar la el modelo para que me
         cree la tablade ountries.*/
        public DbSet<Country> Countries { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }
    }
}
