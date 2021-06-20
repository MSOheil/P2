using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RShop.Data;
using RShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RShop
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
            services.AddControllersWithViews();
            services.AddRazorPages();
            #region DB
            services.AddDbContext<RShopContext_DB>(options =>
            options.UseSqlServer("Data Source=DESKTOP-H3CG3IM\\SQL2019;Initial Catalog=RShop_DB;integrated Security=true;")

            );
            #endregion

            #region AddRepository
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            #region Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/LogOut";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });
            #endregion
            #region Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options=>
            {
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
            }
            
            )
                .AddEntityFrameworkStores<RShopContext_DB>()
                .AddDefaultTokenProviders();
            #endregion



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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/Admin"))
                {
                    if (!context.User.Identity.IsAuthenticated) 
                    {
                    context.Response.Redirect("/Account/Login");
                    }
                    else if(!bool.Parse(context.User.FindFirstValue("IsAdmin")))
                    {
                        context.Response.Redirect("/Account/Login");

                    }

                }
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
