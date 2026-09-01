using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.Repository;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize]
    public class CheckInCheckOutController : ControllerBase
    {
       private readonly ICheckInCheckOutRepository _checkInCheckOutRepository;
        private readonly IEmailService _emailService;
        private readonly IAssignmentRepository _assignmentRepository;
        public CheckInCheckOutController(ICheckInCheckOutRepository checkInCheckOutRepository, IEmailService emailService, IAssignmentRepository assignmentRepository)
        {
            this._checkInCheckOutRepository = checkInCheckOutRepository;
            this._emailService = emailService;
            _assignmentRepository = assignmentRepository;
        }
        [HttpGet, Route("CheckIn/{userId}/{Type}")]
        public IActionResult CheckIn(int userId, string Type)
        {
            var res = this._checkInCheckOutRepository.CheckIn(userId, Type);
            this._assignmentRepository.AutoAllocationLots(userId);           
            return Ok(res);
        }
        [HttpPost, Route("SendFeedBackMail")]
        public async Task<IActionResult> SendFeedBackMail(AutoMailRequest autoMailRequest)
        {
            var status = await _emailService.SendEmailAsync(autoMailRequest.email, autoMailRequest.subject, autoMailRequest.body);
            return Ok(status);
        }

    }
}
