using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Danvic.Vu
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
            //register the swagger document
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info
            {
                Version = "v1",
                Title = "Vu Project API",
                Description = "A front-end separation project using ASP.NET Core Web API",
                Contact = new Contact
                {
                    Name = "Danvic Wang",
                    Email = "danvic96@hotmail.com",
                    Url = "https://github.com/Lanesra712"
                }
            }));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //use static files
            app.UseStaticFiles();

            //Use Swagger to description API 
            app.UseSwagger();

            //Enable middleware to show swagger ui
            /**
             *When start appliaction and go to http://localhost:<port>/swagger/v1/swagger.json
             * the description information will show in the swagger.json
             */
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vu API V1.0");

                //let the api description page in the app's root http://localhost:<port>
                //the default description page in http://localhost:<port>/swagger 
                //c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
