
namespace Shop.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using Shop.Web.Data.Entities;
    using Microsoft.AspNetCore.Http;

    /* 15- Para adicionar o capturar un archivo para el campo Image (local > IIS o en Azure), debemos hacer una extensión de
     * Products (esta clase). Cuando nosotros le decimos al programa CREATE PRODUCT (ProductController) y vamos a su vista
     * correspondiente, nosotros estamos utilizando directamente el Entities.Product, lo cual solo se utiliza cuando esuna 
     * vista sencilla como la que estamos haciendo, pero muchas veces no nos sirve que interactue con el entity, lo mas
     * normal y adecuado es que interactue con el MODELO (el entity lo estamos utilizando como modelo en este caso). Aquí esto
     * no nos servirá, ya que necesitaremos un atributo adicional para capturar la foto, así que creamos esta clase 
     * PRODUCTVIEW MODEL que hereda de PRODUCTS, y le adicionamos el atributo adiconal (IFormFile)
     */
    public class ProductViewModel : Product
    {
        /* 15 - ImageUrl en PRODUCT, es la ruta donde se almacenará la imagen, NO es l aimagen. Hay que capturarlo
         * temporalmente en algo llamado el IFormFile -> (en MVC .net framework esto se hacia mediante un archivo
         * que llama un  ActPostFileBase pero ya cambió, ahora mediante la interfaz/clase IformFile). Con IformFile
         * se puede capturar el archivo en memoria y se puede subir al servidor.
         */
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        /* 15 - Como vamos a cambiar el modelo que mandamos en el método create (PRODUCTSController), enla vista dedicho create
         * GOT OF VIEW, del DATA-ENTIIES se esta recibiendo products, eso lo cambiamos, ya el modelo lo va a recibir 
         * de Models.ProductViewModel 
         Luego de cambiar esto, modificamos el ProductsController en el POST products/create, UNO SIEMPRE RECIBE LO QUE
         MANDA, es decir, si el métdo create recibe un producviewmodel, devuelve un productviewmodel, y esta devolviendo
         un PRODUCT -> create(ProductViewMOdel product)
         */

    }
}
