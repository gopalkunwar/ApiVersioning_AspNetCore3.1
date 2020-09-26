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

            });

            //VERSIONING Type

            // 1. VERSIONING Via QUERY STRING PARAMETER
            //Query string parameter is the default method, for that use a query string parameter named 'api-version'.
            //The default setting is that API version will be a query string parameter named 'api-version' i.e. /cars?api-version=1.1
            // To change query string parameter name of the default setting, We have used QueryStringApiVersionReader("v") i.e. /cars?v=1.1
            // We have used the letter 'v' instead of default 'api-version').
            //  services.AddApiVersioning(options =>
            //       options.ApiVersionReader = new QueryStringApiVersionReader("v"));

            //2. VERSIONING Via HTTP HEADER
            //This option allows to store the API version in the HTTP header
            //  services.AddApiVersioning(options =>
            //        options.ApiVersionReader = new HeaderApiVersionReader("api-version"));

            //3. VERSIONING Via URL PATH
            //To set the versioning our API with URL path segment, beside [ApiVersion(...)] attributes
            //we also need to set the URL segment of the route where the API version will be read from
            //[Route("api/v{version:apiVersion}/[controller]")]
            //  services.AddApiVersioning(options =>
            //        options.ApiVersionReader = new UrlSegmentApiVersionReader());

            //4. API Version Reader Composition=>Combination of Query String and Header
                 services.AddApiVersioning(options => {
                        options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("v"),
                                                                             new HeaderApiVersionReader("v"));
                    });

            //5. API Version Via Convention
            //api/values1?v=3.1
                services.AddApiVersioning(options =>
                {
                    options.Conventions.Controller<Values1Controller>().HasApiVersion(3, 1);
                });

            //6. API Version Via Convention
            //Setting deprecated API version as well as versioning controller actions
                services.AddApiVersioning(options=>
                {
                    options.Conventions.Controller<Values2Controller>().HasDeprecatedApiVersion(3, 0)
                                                                        //if we have two action without specifying the action to 
                                                                        // particular version will result error saying
                                                                        //AmbiguousMatchException: The request matched multiple endpoints. Matches;
                                                                        //API.Controllers.Values2Controller.Get (API)
                                                                        // API.Controllers.Values2Controller.Get1(API)

                                                                        //Will result Get() action from Values2 controller
                                                                        .HasApiVersion(3, 3)

                                                                        ///api/values2?v=3.1
                                                                        .Action(c => c.Get_31()).MapToApiVersion(3,1)

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
