using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.IBankNonInvoice;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartialSalaryReleaseStatusController : ControllerBase
    {
        private readonly IPartialSalaryReleaseStatusRepository _partial;
        //public PartialSalaryReleaseStatusController(
        //  IPartialSalaryReleaseStatusRepository iclient)
        //{
        //    //this._partial = ipartial;
        //}



    }
}
