
namespace Shop.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage ="The field {0} only can contain {1} characters lenght.")]
        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; }

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }

        [Display(Name = "Is Availabe?")]
        public bool IsAvailabe { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        /* Se adiciona el objeto User aquí, en el lado Varios de la relación => 1 Usuario tiene Varios Productos
        Luego hacemos drop de base de datos y realizamos la nueva migración, la cual me creará las tablas
        que en conjunto me manejan la seguridad del sistema. Luego modificamos el SeedDb adicionandole otra
        inyección en SeedDb.cs*/
        public User User { get; set; }

        /*17-230719, Propiedad de lectura, es decir, propiedad GET (borramos el set). Las propiedades de lectura
         no modifican la base de datos, no se necesita realizar migraciòn.Expandimos el GET con un poco de lògica.
         SI IMageUrl está vacia, al IMAGEFullpath le retornamos un null (no hay imagen), si por el contrario si
         tiene, le vamos a retornar la interpolación ($ = sintaxis interpolación), concatenamos la ruta de la imagen
         con la imagen, como inicia con virgulilla le hago un substring(1), inicia a partir de la segunda posic, la
         cero la elimina*/
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }
                return $"https://shopjedbelgarcia.azurewebsites.net{this.ImageUrl.Substring(1)}";
                /*17 return $".....azurewebsites.net" + this.ImageUrl.Substring(1)es lo mismo de arriba, pero esto es muy feo
                 y l ainterpolación es mas poderos, puedes editar, añadir separadores de mil, etc. Ahora vamos
                 ahacer una mejora, ordenar los productos por orden alfabético, ya que al generalizarlo <T> se me dañó
                 el orden. Para esto, debemos crear una propiedad personalizada e implementar métodos personalizados, los 
                 cuales los debo crear en cada repositorio específico; esto lo hago en en Data>IproductRepository (adic metods)
                 Y los métodos que implemente allí a quien le van a servir, solamente a Products. Adicionamos método
                 GetAllWithUsers*/
            }


        }
    }

}
