# ApiVersioning_AspNetCore3.1
Before starting API development, we should have an API versioning strategy that we are going to implement on our API end points.

When we expose our API for use, it is assumed that we have clearly defined the contracts and that the consumers of our API can rely on these contracts. If we make some changes to our API contracts, these should be exposed as a new version of our API. This new version does not mean a new code base of the API. It should be the same code base that supports different versions of an API.

There are many approaches we can use to implement API versioning. In general, there isn't a good or a bad way when talking about API versioning. Which approach to choose depends on who is using our API, on our overall product development situation (are we starting a new project, or do we need to incorporate versioning to a mature, production API), and on how detailed we want to be with API versioning. Find an option that best suits our needs, and stick with it when building our API endpoints.

## install Microsoft's versioning package
  Install-Package Microsoft.AspNetCore.Mvc.Versioning
  
    services.AddApiVersioning(options=> {
                //To activate the default API version
                options.AssumeDefaultVersionWhenUnspecified = true;

                //To set the default API version
                options.DefaultApiVersion = new ApiVersion(1, 0);                

                //Enabling this option, responses from our API endpoints inform which versions are supported or deprecated
                options.ReportApiVersions = true;              

     });
     
### options.DefaultApiVersion = new ApiVersion(1, 0);
With DefaultApiVersion being set, all controllers that do not have an API version attribute ([ApiVersion("1.0")]) applied on them, will implicitly bound to this default API version.

### options.AssumeDefaultVersionWhenUnspecified = true;
DefaultApiVersion sets just the default API version, but we still need to set AssumeDefaultVersionWhenUnspecified to true for default API version to be activated.

### options.ReportApiVersions = true;
Reporting API versions is disabled by default. With enabling this option, responses from our API endpoints will carry a header telling our clients which versions are supported or deprecated (api-supported-versions: 1.1, 2.0, api-deprecated-versions: 1.0).


## API versioning approach
### API Version Reader
API Version Reader defines how an API version is read from the HTTP request. If not explicitly configured, the default setting is that our API version will be a query string parameter named 'api-version'  (example: ../users?api-version=2.0 ). Another, probably more popular option is to store the API version in the HTTP header. We have also the possibility of having an API version both in a query string as well as in an HTTP header.


#### Query String Parameter 
    services.AddApiVersioning(options=>{
                  options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);                
                options.ReportApiVersions = true; 
                options.ApiVersionReader= new QueryStringApiVersionReader("v"));
    });
   
#### HTTP Header
     services.AddApiVersioning(options=>{
                  options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);                
                options.ReportApiVersions = true; 
                options.ApiVersionReader= new HeaderApiVersionReader("api-version"));
    });
        
#### Composite Reader
    services.AddApiVersioning(options=>{
                  options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);                
                options.ReportApiVersions = true; 
                options.ApiVersionReader= ApiVersionReader.Combine(new QueryStringApiVersionReader("v"),new HeaderApiVersionReader("v"));
    });
        
#### URL Path Reader
      services.AddApiVersioning(options=>{
                  options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);                
                options.ReportApiVersions = true; 
                options.ApiVersionReader= new UrlSegmentApiVersionReader("v"));
    });
        
#### Media Type
   In GET Response
   Set Header=> Accept: application/json;v=1.1
   
    services.AddApiVersioning(options=>{
                  options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);                
                options.ReportApiVersions = true; 
                options.ApiVersionReader=new MediaTypeApiVersionReader("v");
    });
    
   
    
* For URL Path Reader: Set the URL segment of the route where the API version will be read, use [Route("api/v{version:apiVersion}/[controller]")] instead of [Route("api/[controller]")].
* Use [ApiVersion("1.0")] means it supports Api version 1.0.
* Use [MapToApiVersion("2.0")] to support only for Api version 2.0.
* Use [ApiVersion("1.0", Deprecated = true)] to deprecate some Api version.


## API Versioning Via Convention
      services.AddApiVersioning( options =>
      {
          options.Conventions.Controller<ValueController>().HasApiVersion(1, 0);
      });
  
 To set deprecated Api version as well as versioning controller actions
 
      services.AddApiVersioning( options =>
        {
              options.Conventions.Controller<MyController>()	   
                                 .HasDeprecatedApiVersion(1, 0)
                                 .HasApiVersion(1, 1)
                                 .HasApiVersion(2, 0)
                                 .Action(c => c.Get1_0()).MapToApiVersion(1, 0)
                                 .Action(c => c.Get1_1()).MapToApiVersion(1, 1)
                                 .Action(c => c.Get2_0()).MapToApiVersion(2, 0);
        });
        
  Using Custom Convention
      
      options.Conventions.Add(new TestCustomConvention());
      
 Using Namespace Convention
      
      options.Conventions.Add(new VersionByNamespaceConvention());
