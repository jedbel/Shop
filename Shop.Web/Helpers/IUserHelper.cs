
namespace Shop.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;

    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        /*Este mètodo AddUser Async le pasamos el usuario y la contraseña y èl me retorna el IdentityResult, el cual
         me dice si lo pudo/no pudo realizar y por qué*/
        Task<IdentityResult> AddUserAsync(User user, string password);
    }

}
