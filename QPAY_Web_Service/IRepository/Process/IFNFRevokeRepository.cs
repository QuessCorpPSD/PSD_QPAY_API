using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.IRepository.Process
{
    public interface IFNFRevokeRepository
    {
        Task<ProcessResponse> ImportFNFRevoke(IFormFile file, [FromForm] string User);
    }
}
