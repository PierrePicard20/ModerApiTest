using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModerApiTest.DAL.Collections;
using ModerApiTest.DAL.Services;
using ModerApiTest.Authentication;
using ModerApiTest.DAL;
using ModerApiTest.Managers;

namespace ModerApiTest
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
            services.AddControllers();

            var connectionString = Configuration["ContainerConnectionString"] ??        // takes the configuration from docker-compose.override.yml (when launched by docker compose) ...
                                   Configuration["DatabaseSettings:ConnectionString"];  // ... otherwise from appsettings.json (when launched as a project)

            services.AddSingleton<IDatabaseContext>(_ => new DatabaseContext( connectionString
                                                                            , Configuration["DatabaseSettings:DatabaseName"]));
            services.AddSingleton<ICollectionService<UserDocument>, UserCollectionService>();
            services.AddSingleton<ICollectionService<ArticleDocument>, ArticleCollectionService>();
            
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IArticleManager, ArticleManager>();

            JWTAuthenticationStrategy.ConfigureAuthenticationService(services, Configuration);
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
                app.UseExceptionHandler("/error");
            }
            app.UseRouting();
            app.UseAuthentication();
            JWTAuthenticationStrategy.UsePostAuthentication(app);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
