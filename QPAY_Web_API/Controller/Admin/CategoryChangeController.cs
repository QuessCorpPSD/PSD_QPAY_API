using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Admin;
using QPay.UI.Models.Admin;

namespace QPay.API.Controller.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryChangeController : ControllerBase
    {
        private readonly ICategoryChange _irepo;

        public CategoryChangeController(ICategoryChange irepo)
        {
            _irepo = irepo;
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody] CategoryChangeModel model)
        {
            var result = await _irepo.SearchCategoryChange(model);
            return Ok(result);
        }

        [HttpPost("Import")]
        public async Task<IActionResult> Import([FromBody] CategoryChangeModel model)
        {
            var result = await _irepo.ImportCategoryChange(model);
            return Ok(result);
        }

    }
}