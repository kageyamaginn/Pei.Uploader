using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pei.BolUploader.Entities;

namespace Pei.BolUploader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BolController : ControllerBase
    {
        public void Post(BolItem item)
        {
            
        }
    }
}