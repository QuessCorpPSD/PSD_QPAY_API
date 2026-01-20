using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Process;

namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class FNFRevokeController : ControllerBase
    {
        private readonly IFNFRevokeRepository _processRepository;
        public FNFRevokeController(IFNFRevokeRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost]
        [Route("ImportFNFRevoke")]
        public async Task<IActionResult> ImportFNFRevoke(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.ImportFNFRevoke(file, User);
            return Ok(result);
        }
    }
}


