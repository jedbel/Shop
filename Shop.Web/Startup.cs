

namespace Shop.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Data;
    using Data.Entities;
    using Helpers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Se hace la inyección del DataContex o DB para que use la configuración de base de datos 
        // configurada en el archico appsettings.json. Cualquier clase que en constructor llame un DbContext lo toma de acá.
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            /*Aqui vamos a usar un Identity, personalisando los usuarion con mis usuarios (mi definicion) y 
             definimos nuestras configuraciones de rol de usuario. Luego adiconalos "app.UseAuthentication"
             (para definir que tiene autenticacion)*/
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 7;
            })
            .AddEntityFrameworkStores<DataContext>();


            // Hace la inyección del SeedDd para que reconozca la clase.
            services.AddTransient<SeedDb>();

            /*Cada que llamen un IRepository me va a inyectar la implementación de la clase Repository 
            AddTransient => se usa y se destruye, AddScoped se reusa las veces que se considere neces*/

            /*Ahora cambiamos nuestra inyecciòn, ya que emininamos el Repository (metodos de produst) por el repositorio genèrico
            tanto para product, cuntries, y para cualquier otra inyecciòn que deseemos adicionar. De aquí ir a ProductsController
            ya que estaba usando el IRepository que acabamos de eliminar*/
            //services.AddScoped<IRepository, Repository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();

            /*Cada que llamen un IUserHelper me va a inyectar la implementación de la clase UserHelper. Después
             de esto vaos a remover la inyeccción del UserManager y cambiarla por la inyección del UserHelper (new generic inyection)*/
            services.AddScoped<IUserHelper, UserHelper>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //se adicionó ya que cree usuario para autenticación
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
