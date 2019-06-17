
namespace Shop.Web.Data.Entities
{
    using Microsoft.AspNetCore.Identity;

    /*Heredamos de IdentityUser, una clase o lògica que nos brinda para elmanejo de usuario. Procedemos a personalizarla.
    De aquí vamos a la clase DataContext y la modificamos, está heredando del DbContext tradicional (no le importan
    las tablas de usuarios), ahora vamos a configurarlo para que herede del IdentityDbContex (otro contexto)*/
    public class User :  IdentityUser 
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
