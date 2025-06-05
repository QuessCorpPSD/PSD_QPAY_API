using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInCheckOutController : ControllerBase
    {
       private readonly ICheckInCheckOutRepository _checkInCheckOutRepository;
        public CheckInCheckOutController(ICheckInCheckOutRepository checkInCheckOutRepository)
        {
            this._checkInCheckOutRepository = checkInCheckOutRepository;
        }
        [HttpGet, Route("CheckIn/{userId}/{Type}")]
        public IActionResult CheckIn(int userId, string Type)
        {
            var res = this._checkInCheckOutRepository.CheckIn(userId, Type);
            return Ok(res);
        }
        
    }
}
