
namespace Shop.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;

    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> userManager;

        /*Esta es la inyeccción del userManager, la cuál no se debe configurar en el startUp ya que viene inplicita/dada
         en el Core*/
        public UserHelper(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        /*Le pasamos el usuario y passw, retorna IdentityResult*/
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
        }

        /*Me retorna un Usuaro de mi clase extendidda/Heredada User del Usuario Identity*/
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await this.userManager.FindByEmailAsync(email);
             
        }
        /*Como esta clase es mia, debo instanciarla o matricularla en mi StartUp, cambiar la instancia de la Clase
         Repositorio, ya que vamos a crear una clase genérica, de esta manera no crear los mismos metodos para crear
         producto, un usuario, un pais, sino generalizarlo en una sola clase.*/
    }

}
