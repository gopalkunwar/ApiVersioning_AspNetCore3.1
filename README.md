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
