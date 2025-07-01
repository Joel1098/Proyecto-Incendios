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
using Microsoft.OpenApi.Models;
using Forestry.Services;

namespace Forestry
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // This method gets called by the runtime. Use this method to add services to the container.
            // ConfigureServices
            // Configurar CORS para permitir peticiones desde Angular
            builder.Services.AddCors(options =>
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

            builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", configuraciones => {

                configuraciones.Cookie.Name = "Forestry";
                configuraciones.ExpireTimeSpan = TimeSpan.FromDays(1);
                configuraciones.LoginPath = "/Home/Login";
                configuraciones.AccessDeniedPath = "/Home/AccesDenied";
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(10);
            });

            // Configurar para API REST
            builder.Services.AddControllers();

            // --- INICIO: Lógica para Render DATABASE_URL ---
            string GetConnectionString(IConfiguration config)
            {
                // 1. Intenta obtener la cadena de conexión de variable de entorno (Render/Heroku)
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                if (!string.IsNullOrEmpty(databaseUrl))
                {
                    // Ejemplo: postgres://usuario:password@host:puerto/db
                    var uri = new Uri(databaseUrl);
                    var userInfo = uri.UserInfo.Split(':');
                    var builder = new Npgsql.NpgsqlConnectionStringBuilder
                    {
                        Host = uri.Host,
                        Port = uri.Port,
                        Username = userInfo[0],
                        Password = userInfo[1],
                        Database = uri.AbsolutePath.TrimStart('/'),
                        SslMode = Npgsql.SslMode.Require,
                        TrustServerCertificate = true
                    };
                    return builder.ToString();
                }
                // 2. Si no existe DATABASE_URL, usa la cadena de conexión por defecto
                return config.GetConnectionString("DefaultConnection");
            }
            // --- FIN: Lógica para Render DATABASE_URL ---

            builder.Services.AddDbContext<ContextoBaseDeDatos>(opt =>
            {
                var connectionString = GetConnectionString(builder.Configuration);
                opt.UseNpgsql(connectionString);
            });

            builder.Services.AddHttpContextAccessor();

            //-------------------------------
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".YourApp.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configurar Email Service
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();

            // Configurar Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Forestry API", 
                    Version = "v1",
                    Description = "API REST para Gestión de Incendios Forestales",
                    Contact = new OpenApiContact
                    {
                        Name = "Forestry Team",
                        Email = "support@forestry.com"
                    }
                });
            });

            var app = builder.Build();

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            // Configure
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forestry API v1");
                    c.RoutePrefix = string.Empty; // Esto hace que Swagger esté disponible en la raíz
                });
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

            app.MapControllers();

            app.Run();
        }
    }
}
