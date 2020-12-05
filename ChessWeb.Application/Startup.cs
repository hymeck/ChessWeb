using ChessWeb.Domain.Entities;
using ChessWeb.Domain.Interfaces.Repositories;
using ChessWeb.Persistence.Contexts;
using ChessWeb.Persistence.Implementations;
using ChessWeb.Service.Interfaces;
using ChessWeb.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ChessWeb.Application
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddTransient<IUserValidator<User>, CustomUserValidator>();
 
            services
                .AddDbContext<ApplicationDbContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
 
            services.AddIdentity<User, UserRole>(opts=> {
                    opts.Password.RequiredLength = 5;   // минимальная длина
                    opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                    opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                    opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                    opts.Password.RequireDigit = false; // требуются ли цифры
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            
            services.AddScoped<IChessService, ChessService>();
            services.AddScoped<IGameService, GameService>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IGameStatusRepository, GameStatusRepository>();
            services.AddTransient<IGameSummaryRepository, GameSummaryRepository>();
            services.AddTransient<IMoveRepository, MoveRepository>();
            services.AddTransient<ISideRepository, SideRepository>();

            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseDeveloperExceptionPage();
 
            // app.UseHttpsRedirection();
            app.UseStaticFiles();
 
            app.UseRouting();
            app.UseAuthentication();    // подключение аутентификации
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}