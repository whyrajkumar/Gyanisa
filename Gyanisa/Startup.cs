using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gyanisa.Data;
using Gyanisa.Models;
using Gyanisa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Gyanisa.Data.Interface;
using Gyanisa.Data.Repository;
using Gyanisa.Sitemap;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Gyanisa.Middleware;

using Gyanisa.Extensions;
using Gyanisa.Data.FileManager;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.ResponseCompression;


namespace Gyanisa
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer("Server=./;Database=Gyanisa;Trusted_Connection=True;MultipleActiveResultSets=true",
            //optionsBuilder => optionsBuilder.MigrationsAssembly("Gyanisa")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Read Email Setting
            services.Configure<EmailConfig>(Configuration.GetSection("EmailConfig"));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<ISubjectCategoryRepository, SubjectCategoryRepository>();
            // services.AddScoped(typeof(IAllRepository<>), typeof(AllRepository<>));
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IFileManager, FileManager>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //nuget pagcakge for Pagination
           
             
            services.AddOptions();
            services.AddResponseCaching();
            services.AddMemoryCache();
            //services.AddResponseCompression();
            //services.AddResponseCompression( options=>
            //{
            //    IEnumerable<string> MimeTypes = new[]
            //    {
            //        //general
            //         "text/plain",
            //         "text/html",
            //         "text/css",
            //         "font/woff2",
            //         "application/javascript",
            //         "image/x-icon",
            //         "image/png"
            //    };
            //    options.EnableForHttps = true;
            //    options.ExcludedMimeTypes = MimeTypes;
            //    options.Providers.Add<GzipCompressionProvider>();
            //    options.Providers.Add<BrotliCompressionProvider>();

            //});
            services.AddMvc();
            services.AddApplicationInsightsTelemetry();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProviders)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else if(env.IsProduction() || env.IsStaging() || env.IsEnvironment("Staging_2"))
            {
                 app.UseExceptionHandler("/Error");               
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
           
            //app.UseResponseCompression();
            app.UseBrowserLink();
            app.UseCookiePolicy();
            app.UseHttpContext();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;


                    string path = ctx.File.PhysicalPath;
                    if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".svg"))
                    {
                        TimeSpan maxAge = new TimeSpan(7, 0, 0, 0);
                        ctx.Context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
                    }
                }
            });
            app.UseAuthentication();
            // app.UseSitemapMiddleware();

            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromHours(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                      template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Generate UserRole at application start up
         RolesInit(serviceProviders).Wait();
        }

        private  async Task RolesInit(IServiceProvider serviceProvider)
        {
            //adding custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Manager", "Member","Tutor","Student", "Institute" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }
           

            var poweruser = new ApplicationUser
            {
                UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                Email = Configuration.GetSection("UserSettings")["UserEmail"]
            };
            string UserPassword = Configuration.GetSection("UserSettings")["UserPassword"];
            var _user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);
            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }

    }
}
