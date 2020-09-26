using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Values2Controller : ControllerBase
    {
        //api/values2?v=3.3
        public string Get()
        {
            return "Setting Convention,  deprecated API version as well as versioning controller actions: Get()";
        }
        //public string Get1()
        //{
        //    return "Setting Convention,  deprecated API version as well as versioning controller actions: Get1()";
        //}
        //api/values2?v=3.1
        public string Get_31()
        {
            return "Map Controller Action to particular API version using .Action(c => c.Get_31()).MapToApiVersion(3, 1) in Convention API Versioning";
        }

        //api/values2? v = 3.2
        public string Get_32()
        {
            return "Map Controller Action to particular API version using .Action(c => c.Get_32()).MapToApiVersion(3, 2) in Convention API Versioning";
        }
    }
}
