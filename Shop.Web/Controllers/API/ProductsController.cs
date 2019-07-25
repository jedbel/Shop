
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;
    using Shop.Web.Data;

    /*Este es el controlador de la API(Interfaz que se genera para que otros proyectos la consuma, ej consuman mis productos)
     El controlador de la API nunca tiene vistas*/

    /*cuando yo publique mi sitio web, a direcciòn que le pongamos + /api/Controller, me va a cceder los datos de este controlador*/
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        /*Como también este controlador necesita los datos de los productos, le vamos a inyectar el ProductsRepository (se le inyecta con el constructor)*/
        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        /*Vamos a crear nuestro método, colo lo llamé GetProducts, por inferencia de métodos Core sabe que es un metodo Get
         ya que le puse "GetXXXX". Sen embargo por notación se le adiciona el HttpGet. Los productos los retorno envolviendo
         el resultado en un JSON y eso es lo que devuelve (usando return OK(), crear y sonsu,ir servicios restful es muy sencillo)
         Pero yo no quiero que me devuelva un OK (un json vacio), sino la lista de productos, para lo cual ya tengo un método
         creado con anterioridad, */
        [HttpGet]
        public IActionResult GetProducts()
        {
            /*17 cambio el GetAll por el GetAllWithUsers, y como voy a mostrar la lista en el teléfono, elteléfono puede
             ordenar, pero se recomienda que el trabajo pesado me lo haga el backed, dejando la app movil lo mas liviano
             posible, así que ordeno por nombre (error y no se ordena, esperar sino borrar comentario.)*/
            return Ok(this.productRepository.GetAllWithUsers());
        }
    }
}
