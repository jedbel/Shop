using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    using Entities;

    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        /*ProductRepository inyecta el DataContext y se lopasa al controctor de la suplercalse o clase base*/
        public CountryRepository(DataContext context) : base(context)
        {
        }
    }
     


        
  
}
