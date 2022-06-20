using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace ServerApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _currentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\cards.json";
            if (File.Exists(path))
            {
                services.AddTransient<IRepository<Book>>(x => new BookRepository<Book>(path));
            }
            services.AddTransient<FileSaverService>(x => new FileSaverService(_currentEnvironment));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
