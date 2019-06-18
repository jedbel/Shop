using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    using Entities;

    /*La interfaz de Productos (IProductRepositor), va a ser una implementación de la interfaz genérica IGenericRepository,
     * pero con paises*/
    public interface ICountryRepository : IGenericRepository<Country>
    {
    }

}
