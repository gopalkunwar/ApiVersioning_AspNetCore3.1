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
    //Api version not specified then default api version [ApiVersion("1.0")] implicitly called
    public class ValuesController : ControllerBase
    {
        public String Get()
        { 
            return "Version Not Specified";
        }
    }
}
