using Biblio.Web.DATA;

namespace Biblio.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            
            // Add services to the container.
            // aqui es que se registran todas las inyecciones de dependencias
            builder.Services.AddScoped<ICategoriaDao, CategoriaDao>();
            builder.Services.AddScoped<ILibrosDao, LibrosDao>();
            builder.Services.AddScoped<IPrestamoDao, PrestamoDao>();
            builder.Services.AddScoped<IEstadoPrestamoDao, EstadoPrestamoDao>();
            builder.Services.AddScoped<IRolDao, RolDao>();
            builder.Services.AddScoped<IUsuarioDao, UsuarioDao>();
            builder.Services.AddScoped<IPenalizacionDao, PenalizacionDao>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
