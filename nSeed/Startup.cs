using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using nSeed.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace nSeed
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // TODO refactor this into something more useable 
            Global.Utils.TorrentSearchResultReader.InitConfiguration(Configuration);
            Global.Utils.SystemInformation.InitConfiguration(Configuration);
            Global.Utils.TorrentDetailReader.InitConfiguration(Configuration);
            Global.Utils.TorrentNFOReader.InitConfiguration(Configuration);
            Global.Utils.TorrentCommentsReader.InitConfiguration(Configuration);
            // TODO STARTUP CODE HERE, INITIALIZING QBIT AND HTTPHANDLER
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();

            // TODO testing with this
            services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequiredLength = 10;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                    options.Lockout.MaxFailedAccessAttempts = 3;
                }
            );


            services.AddAuthorization(options => {
                options.AddPolicy("RequireAdministratorRole",
                    builder => builder.RequireRole("admin"));
            });

            var cookiecontainer = new CookieContainer();
            services.AddSingleton(cookiecontainer);

            services.AddHttpClient("myhttpclient")
                .ConfigurePrimaryHttpMessageHandler((c) => new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    //MaxAutomaticRedirections = 5,
                    UseCookies = true,
                    CookieContainer = cookiecontainer,
                });

            // use : var cont = HttpContext.RequestServices.GetService<CookieContainer>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
