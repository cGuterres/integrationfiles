using IntegrationFile.Domain.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IntegrationFiles.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessFileController : ControllerBase
    {
        private readonly IProcessFileService _service;

        public ProcessFileController(IProcessFileService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public IActionResult ProcessFile()
        {
            var result =_service.ReadFiles();
            
            return Ok(result);
        }
    }
}
