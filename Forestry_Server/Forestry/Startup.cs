using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
//using Forestry.Repositories;
using Forestry.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Forestry
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurar CORS para permitir peticiones desde Angular
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200") // Puerto por defecto de Angular
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });

            services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", configuraciones => {

                configuraciones.Cookie.Name = "Forestry";
                configuraciones.ExpireTimeSpan = TimeSpan.FromDays(1);
                configuraciones.LoginPath = "/Home/Login";
                configuraciones.AccessDeniedPath = "/Home/AccesDenied";
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(10);
            });

            // Configurar para API REST
            services.AddControllers();

            services.AddDbContext<ContextoBaseDeDatos>(opt =>
            {
                // Para usar PostgreSQL en Docker, la cadena de conexión debe ser compatible
                opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHttpContextAccessor();

            //-------------------------------
            services.AddSession(options =>
            {
                options.Cookie.Name = ".YourApp.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(20); // Puedes ajustar el tiempo de expiración según tus necesidades
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection(); //checar
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();
            
            // Habilitar CORS
            app.UseCors("AllowAngularApp");
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
