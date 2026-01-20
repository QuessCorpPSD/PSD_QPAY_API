using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface IBandRepository
    {
        Task<List<Band>> GetAllBandDetails(string companyId);
        Task<BandResponse> SaveUpdateDeleteBand([FromBody] BandRequest request);

    }
}
