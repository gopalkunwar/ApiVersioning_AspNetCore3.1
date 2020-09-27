using API.Controllers;
using Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
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
            services.AddDbContext<ApplicationDbContext>(options=> 
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.AddApiVersioning(options=> {
                //To activate the default API version
                options.AssumeDefaultVersionWhenUnspecified = true;

                //To set the default API version
                options.DefaultApiVersion = new ApiVersion(1, 0);                

                //Enabling this option, responses from our API endpoints inform which versions are supported or deprecated
                options.ReportApiVersions = true;

                //Query String Parameter
                //Changed parameter reading from "api-version" to "v"
                //api/cars?v=1.2
                //options.ApiVersionReader = new QueryStringApiVersionReader("v");

                //Header Parameter
                //api/cars
                //In Header=>Set api-version=1.2
                //options.ApiVersionReader = new HeaderApiVersionReader("api-version");

                //URL Path
                //api/v1.1/cars
                //Set [Route("api/v{version:apiVersion}/[controller]")] in Controller
                //options.ApiVersionReader = new UrlSegmentApiVersionReader();

                //Media Type
                //In Header=> Set Accept:application/json;v=1.1
                //options.ApiVersionReader = new MediaTypeApiVersionReader("v");

                //Combination of Query String & Header
                //Works both on api/v1.1/cars?v=1.1 OR
                //In Header=> Set v=1.1
                //options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("v"),
                //                                              new HeaderApiVersionReader("v"));

                options.Conventions.Controller<Values2Controller>().HasDeprecatedApiVersion(3, 0)
                                                                        //if we have two action without specifying the action to 
                                                                        // particular version will result error saying
                                                                        //AmbiguousMatchException: The request matched multiple endpoints. Matches;
                                                                        //API.Controllers.Values2Controller.Get (API)
                                                                        // API.Controllers.Values2Controller.Get1(API)

                                                                        //Will result Get() action from Values2 controller
                                                                        .HasApiVersion(3, 3)

                                                                        ///api/values2?v=3.1
                                                                        .Action(c => c.Get_31()).MapToApiVersion(3, 1)

                                                                        ///api/values2?v=3.2
                                                                        .Action(c => c.Get_32()).MapToApiVersion(3, 2);

                //API Versioning via Custom Convention
                //options.Conventions.Add(new ApiVersioning_CustomConvention());

                //API Versiong by namespace convention
                //options.Conventions.Add(new VersionByNamespaceConvention());
            });   


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
