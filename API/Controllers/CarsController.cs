using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
    //To deprecate some version in our API controller, we need to set Deprecated flag to true: [ApiVersion("1.0", Deprecated = true)].
    [ApiVersion("1.1", Deprecated =true)]
    [ApiVersion("1.2")]
    [ApiVersion("1.3")]
    [Route("api/[controller]")]

    //To set the versioning our API with URL path segment, beside [ApiVersion(...)] attributes
    //we also need to set the URL segment of the route where the API version will be read from
    //[Route("api/v{version:apiVersion}/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetAll()
        {
            var cars = await _context.Cars.ToListAsync();
            return Ok(cars);
        }

        [MapToApiVersion("3.0")]
        public string Get()
        {
            return "Specific version number";
        }
    }
}
